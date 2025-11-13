using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using VisionCare.Application.DTOs.BookingDto;
using VisionCare.Application.Interfaces.Booking;
using VisionCare.WebAPI.Hubs;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<BookingHub> _hubContext;
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingHoldCacheService _holdCache;

    public BookingController(
        IBookingService bookingService,
        IConfiguration configuration,
        IHubContext<BookingHub> hubContext,
        ILogger<BookingController> logger,
        IBookingHoldCacheService holdCache
    )
    {
        _bookingService = bookingService;
        _configuration = configuration;
        _hubContext = hubContext;
        _logger = logger;
        _holdCache = holdCache;
    }

    [HttpGet("available-slots")]
    public async Task<ActionResult<IEnumerable<AvailableSlotDto>>> GetAvailableSlots(
        [FromQuery] int doctorId,
        [FromQuery] DateOnly date,
        [FromQuery] int? serviceTypeId = null
    )
    {
        try
        {
            var slots = await _bookingService.GetAvailableSlotsAsync(doctorId, date, serviceTypeId);
            return Ok(slots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available slots");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPost("hold-slot")]
    public async Task<ActionResult<HoldSlotResponse>> HoldSlot([FromBody] HoldSlotRequest request)
    {
        try
        {
            var response = await _bookingService.HoldSlotAsync(request);

            var holdData = await _holdCache.GetHoldAsync(response.HoldToken);

            var groupName = $"slots:{request.DoctorId}:{request.ScheduleDate:yyyyMMdd}";
            await _hubContext
                .Clients.Group(groupName)
                .SendAsync(
                    "SlotHeld",
                    new
                    {
                        doctorId = request.DoctorId,
                        slotId = request.SlotId,
                        date = request.ScheduleDate.ToString("yyyyMMdd"),
                        holdToken = response.HoldToken,
                        expiresAt = response.ExpiresAt,
                        customerId = holdData?.CustomerId,
                    }
                );

            return Ok(response);
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error holding slot");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPost("release-hold")]
    public async Task<ActionResult> ReleaseHold([FromBody] ReleaseHoldRequest request)
    {
        try
        {
            await _bookingService.ReleaseHoldAsync(request.HoldToken);
            await _holdCache.RemoveBySlotAsync(
                request.DoctorId,
                request.SlotId,
                request.ScheduleDate
            );

            var groupName = $"slots:{request.DoctorId}:{request.ScheduleDate:yyyyMMdd}";
            await _hubContext
                .Clients.Group(groupName)
                .SendAsync(
                    "SlotReleased",
                    new
                    {
                        doctorId = request.DoctorId,
                        slotId = request.SlotId,
                        date = request.ScheduleDate.ToString("yyyyMMdd"),
                    }
                );

            return Ok(new { message = "released" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error releasing hold");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPost("create")]
    public async Task<ActionResult<BookingResponse>> CreateBooking(
        [FromBody] CreateBookingRequest request
    )
    {
        if (request == null)
        {
            _logger.LogWarning("CreateBooking: Request is null");
            return BadRequest(new { message = "Request body is required" });
        }

        _logger.LogInformation(
            "CreateBooking request: DoctorId={DoctorId}, ServiceDetailId={ServiceDetailId}, SlotId={SlotId}, ScheduleDate={ScheduleDate} (default={IsDefault}), StartTime={StartTime} (default={IsTimeDefault}), CustomerId={CustomerId}, Phone={Phone}, Email={Email}",
            request.DoctorId,
            request.ServiceDetailId,
            request.SlotId,
            request.ScheduleDate,
            request.ScheduleDate == default(DateOnly),
            request.StartTime,
            request.StartTime == default(TimeOnly),
            request.CustomerId,
            request.Phone,
            request.Email
        );

        if (request.ScheduleDate == default(DateOnly))
        {
            _logger.LogWarning("CreateBooking: ScheduleDate is default (not parsed correctly)");
            return BadRequest(
                new { message = "Invalid schedule date format. Expected: YYYY-MM-DD" }
            );
        }

        if (request.StartTime == default(TimeOnly))
        {
            _logger.LogWarning("CreateBooking: StartTime is default (not parsed correctly)");
            return BadRequest(new { message = "Invalid start time format. Expected: HH:mm" });
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x =>
                    x.Value!.Errors.Select(e =>
                        $"{x.Key}: {e.ErrorMessage ?? e.Exception?.Message ?? "Invalid value"}"
                    )
                )
                .ToList();
            _logger.LogWarning("ModelState validation failed: {Errors}", string.Join(", ", errors));
            return BadRequest(new { message = "Validation failed", errors });
        }

        var remoteIp = HttpContext.Connection.RemoteIpAddress;
        string ipAddress = "127.0.0.1";

        if (remoteIp != null)
        {
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                ipAddress = remoteIp.MapToIPv4().ToString();
            }
            else if (remoteIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                ipAddress = remoteIp.ToString();
            }
            else
            {
                ipAddress = remoteIp.ToString();
            }
        }

        _logger.LogDebug("Client IP address for VNPay: {IpAddress}", ipAddress);

        try
        {
            var response = await _bookingService.CreateBookingAsync(request, ipAddress);

            var groupName = $"slots:{request.DoctorId}:{request.ScheduleDate:yyyyMMdd}";

            await _hubContext
                .Clients.Group(groupName)
                .SendAsync(
                    "SlotBooked",
                    new
                    {
                        doctorId = request.DoctorId,
                        slotId = request.SlotId,
                        date = request.ScheduleDate.ToString("yyyyMMdd"),
                        appointmentCode = response.AppointmentCode,
                    }
                );

            await _hubContext
                .Clients.Group("admin:bookings")
                .SendAsync(
                    "BookingCreated",
                    new
                    {
                        appointmentId = response.AppointmentId,
                        appointmentCode = response.AppointmentCode,
                        doctorId = request.DoctorId,
                        totalAmount = response.TotalAmount,
                        paymentStatus = response.PaymentStatus.ToString(),
                    }
                );

            return Ok(response);
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error creating booking: {Message}", ex.Message);
            return BadRequest(
                new
                {
                    message = ex.Message,
                    errors = new System.Collections.Generic.Dictionary<string, string[]>
                    {
                        { "General", new[] { ex.Message } },
                    },
                }
            );
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            _logger.LogWarning(ex, "Not found error creating booking: {Message}", ex.Message);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error creating booking. Request: DoctorId={DoctorId}, ServiceDetailId={ServiceDetailId}, SlotId={SlotId}",
                request.DoctorId,
                request.ServiceDetailId,
                request.SlotId
            );

            var errorMessage = "Đã xảy ra lỗi khi tạo đặt lịch. Vui lòng thử lại sau.";
            var errorDetails = new System.Collections.Generic.Dictionary<string, object>
            {
                { "message", errorMessage },
            };

#if DEBUG
            errorDetails.Add("exception", ex.Message);
            errorDetails.Add("stackTrace", ex.StackTrace);
#endif
            return StatusCode(500, errorDetails);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBookingById(int id)
    {
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            return Ok(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<BookingDto>> SearchBooking(
        [FromQuery] string? appointmentCode = null,
        [FromQuery] string? phone = null
    )
    {
        try
        {
            BookingDto? booking = null;

            if (!string.IsNullOrEmpty(appointmentCode))
            {
                booking = await _bookingService.GetBookingByCodeAsync(appointmentCode);
            }
            else if (!string.IsNullOrEmpty(phone))
            {
                booking = await _bookingService.GetBookingByPhoneAsync(phone);
            }
            else
            {
                return BadRequest(
                    new { message = "Either appointmentCode or phone must be provided" }
                );
            }

            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            return Ok(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching booking");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPut("{id}/cancel")]
    public async Task<ActionResult> CancelBooking(int id, [FromBody] CancelBookingRequest request)
    {
        try
        {
            var success = await _bookingService.CancelBookingAsync(id, request);
            if (!success)
                return BadRequest(new { message = "Failed to cancel booking" });

            return Ok(new { message = "Booking cancelled successfully" });
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPost("{id}/payment/initiate")]
    public async Task<ActionResult<string>> InitiatePayment(int id)
    {
        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            var paymentUrl = await _bookingService.InitiatePaymentAsync(id, ipAddress);
            return Ok(new { paymentUrl });
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating payment");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [AllowAnonymous]
    [HttpGet("payment/callback")]
    [HttpPost("payment/callback")]
    public async Task<ActionResult> PaymentCallback()
    {
        try
        {
            var queryParams = Request.Query.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ToString()
            );

            _logger.LogInformation(
                "Payment callback received with {Count} parameters",
                queryParams.Count
            );

            queryParams.TryGetValue("vnp_ResponseCode", out var rspCode);
            queryParams.TryGetValue("vnp_TransactionStatus", out var txnStatus);
            queryParams.TryGetValue("vnp_TxnRef", out var txnRef);

            var success = await _bookingService.ProcessPaymentCallbackAsync(queryParams);

            if (success)
            {
                if (queryParams.TryGetValue("vnp_TxnRef", out var orderId))
                {
                    var booking = await _bookingService.GetBookingByCodeAsync(orderId);
                    if (booking != null)
                    {
                        await _hubContext
                            .Clients.Group("admin:bookings")
                            .SendAsync(
                                "PaymentProcessed",
                                new
                                {
                                    appointmentId = booking.Id,
                                    appointmentCode = booking.AppointmentCode,
                                    paymentStatus = booking.PaymentStatus,
                                }
                            );
                    }
                }
            }

            var frontendBaseUrl = _configuration["Frontend:BaseUrl"] ?? "http://localhost:5173";

            if (success)
                return Redirect(
                    $"{frontendBaseUrl}/booking/success?code={Uri.EscapeDataString(txnRef ?? string.Empty)}"
                );
            else
                return Redirect(
                    $"{frontendBaseUrl}/booking/failed?rsp={Uri.EscapeDataString(rspCode ?? string.Empty)}&st={Uri.EscapeDataString(txnStatus ?? string.Empty)}&code={Uri.EscapeDataString(txnRef ?? string.Empty)}"
                );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment callback");
            return Redirect("http://localhost:5173/booking/failed");
        }
    }
}
