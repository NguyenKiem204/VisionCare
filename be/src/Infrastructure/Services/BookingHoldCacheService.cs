using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using VisionCare.Application.DTOs.BookingDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.Booking;

namespace VisionCare.Infrastructure.Services;

public class BookingHoldCacheService : IBookingHoldCacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<BookingHoldCacheService> _logger;
    private const string HOLD_KEY_PREFIX = "hold";
    private const string TOKEN_KEY_PREFIX = "hold:token";
    private const string LOCK_KEY_PREFIX = "lock:hold";
    private const int HOLD_DURATION_MINUTES = 10;
    private const int LOCK_DURATION_SECONDS = 5;

    public BookingHoldCacheService(IDistributedCache cache, ILogger<BookingHoldCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<string> CreateHoldAsync(HoldSlotRequest request)
    {
        var holdToken = Guid.NewGuid().ToString("N");
        var cacheKey =
            $"{HOLD_KEY_PREFIX}:{request.DoctorId}:{request.SlotId}:{request.ScheduleDate:yyyyMMdd}";
        var lockKey =
            $"{LOCK_KEY_PREFIX}:{request.DoctorId}:{request.SlotId}:{request.ScheduleDate:yyyyMMdd}";

        var lockAcquired = await TryAcquireLockAsync(
            lockKey,
            TimeSpan.FromSeconds(LOCK_DURATION_SECONDS)
        );
        if (!lockAcquired)
        {
            throw new ValidationException("Slot đang được xử lý, vui lòng thử lại");
        }

        try
        {
            var existing = await _cache.GetStringAsync(cacheKey);
            if (existing != null)
            {
                throw new ValidationException("Slot đang được giữ bởi người khác");
            }

            var holdData = new BookingHoldData
            {
                HoldToken = holdToken,
                DoctorId = request.DoctorId,
                SlotId = request.SlotId,
                ScheduleDate = request.ScheduleDate,
                CustomerId = request.CustomerId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(HOLD_DURATION_MINUTES),
                CreatedAt = DateTime.UtcNow,
            };

            var json = JsonSerializer.Serialize(holdData);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(HOLD_DURATION_MINUTES),
            };

            await _cache.SetStringAsync(cacheKey, json, options);

            var tokenKey = $"{TOKEN_KEY_PREFIX}:{holdToken}";
            await _cache.SetStringAsync(tokenKey, cacheKey, options);

            _logger.LogInformation(
                "Hold created for slot {SlotId} on {Date}",
                request.SlotId,
                request.ScheduleDate
            );
            return holdToken;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Redis unavailable while creating hold. Proceeding without cache."
            );
            return holdToken;
        }
        finally
        {
            try
            {
                await _cache.RemoveAsync(lockKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Redis unavailable removing lock {LockKey}. Ignored.",
                    lockKey
                );
            }
        }
    }

    public async Task<BookingHoldData?> GetHoldAsync(string holdToken)
    {
        var tokenKey = $"{TOKEN_KEY_PREFIX}:{holdToken}";
        string? cacheKey = null;
        try
        {
            cacheKey = await _cache.GetStringAsync(tokenKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Redis unavailable when reading token key {TokenKey}. Treating as no hold.",
                tokenKey
            );
            return null;
        }

        if (string.IsNullOrEmpty(cacheKey))
            return null;

        string? json;
        try
        {
            json = await _cache.GetStringAsync(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Redis unavailable when reading cache key {CacheKey}. Treating as no hold.",
                cacheKey
            );
            return null;
        }
        if (string.IsNullOrEmpty(json))
            return null;

        try
        {
            var holdData = JsonSerializer.Deserialize<BookingHoldData>(json);
            return holdData?.IsExpired() == false ? holdData : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<BookingHoldData?> GetHoldBySlotAsync(
        int doctorId,
        int slotId,
        DateOnly scheduleDate
    )
    {
        var cacheKey = $"{HOLD_KEY_PREFIX}:{doctorId}:{slotId}:{scheduleDate:yyyyMMdd}";
        string? json;
        try
        {
            json = await _cache.GetStringAsync(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Redis unavailable when reading hold by slot {CacheKey}. Treating as no hold.",
                cacheKey
            );
            return null;
        }

        if (string.IsNullOrEmpty(json))
            return null;

        try
        {
            var holdData = JsonSerializer.Deserialize<BookingHoldData>(json);
            return holdData?.IsExpired() == false ? holdData : null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> ValidateHoldAsync(string holdToken)
    {
        var hold = await GetHoldAsync(holdToken);
        return hold != null && !hold.IsExpired();
    }

    public async Task ReleaseHoldAsync(string holdToken)
    {
        var hold = await GetHoldAsync(holdToken);
        if (hold == null)
            return;

        var cacheKey =
            $"{HOLD_KEY_PREFIX}:{hold.DoctorId}:{hold.SlotId}:{hold.ScheduleDate:yyyyMMdd}";
        var tokenKey = $"{TOKEN_KEY_PREFIX}:{holdToken}";

        await _cache.RemoveAsync(cacheKey);
        await _cache.RemoveAsync(tokenKey);

        _logger.LogInformation(
            "Hold released for slot {SlotId} on {Date}",
            hold.SlotId,
            hold.ScheduleDate
        );
    }

    public async Task RemoveBySlotAsync(int doctorId, int slotId, DateOnly scheduleDate)
    {
        var cacheKey = $"{HOLD_KEY_PREFIX}:{doctorId}:{slotId}:{scheduleDate:yyyyMMdd}";
        try
        {
            await _cache.RemoveAsync(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Redis unavailable when removing by slot {CacheKey}", cacheKey);
        }
    }

    public async Task<bool> TryAcquireLockAsync(string lockKey, TimeSpan duration)
    {
        var lockValue = Guid.NewGuid().ToString();
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = duration,
        };

        try
        {
            var existing = await _cache.GetStringAsync(lockKey);
            if (existing != null)
            {
                _logger.LogDebug("Lock {LockKey} already acquired by another process", lockKey);
                return false;
            }

            await _cache.SetStringAsync(lockKey, lockValue, options);

            var verify = await _cache.GetStringAsync(lockKey);
            if (verify == lockValue)
            {
                _logger.LogDebug("Successfully acquired lock {LockKey}", lockKey);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Redis unavailable acquiring lock {LockKey}. Allowing proceed without lock.",
                lockKey
            );
            return true;
        }
    }
}
