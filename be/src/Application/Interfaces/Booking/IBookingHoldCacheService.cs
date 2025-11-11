using VisionCare.Application.DTOs.BookingDto;

namespace VisionCare.Application.Interfaces.Booking;

public interface IBookingHoldCacheService
{
    Task<string> CreateHoldAsync(HoldSlotRequest request); // Returns holdToken
    Task<BookingHoldData?> GetHoldAsync(string holdToken);
    Task<BookingHoldData?> GetHoldBySlotAsync(int doctorId, int slotId, DateOnly scheduleDate); // Get hold by slot
    Task<bool> ValidateHoldAsync(string holdToken);
    Task ReleaseHoldAsync(string holdToken);
    Task RemoveBySlotAsync(int doctorId, int slotId, DateOnly scheduleDate);
    Task<bool> TryAcquireLockAsync(string lockKey, TimeSpan duration); // For race condition
}
