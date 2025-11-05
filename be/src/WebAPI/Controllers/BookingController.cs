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

    /// <summary>
    [HttpPost("hold-slot")]
    public async Task<ActionResult<HoldSlotResponse>> HoldSlot([FromBody] HoldSlotRequest request)
    {
        try
        {
            var response = await _bookingService.HoldSlotAsync(request);

            // Get hold data to include customerId in SignalR event
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
                        customerId = holdData?.CustomerId, // Include customerId for frontend to identify
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
            // Fallback: ensure key by slot is removed even if token not found
            await _holdCache.RemoveBySlotAsync(request.DoctorId, request.SlotId, request.ScheduleDate);

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

    /// <summary>
    /// Create a new booking
    /// </summary>
    [HttpPost("create")]
    public async Task<ActionResult<BookingResponse>> CreateBooking(
        [FromBody] CreateBookingRequest request
    )
    {
        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
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
            return BadRequest(new { message = ex.Message });
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return StatusCode(500, new { message = "Internal server error" });
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

    [HttpPost("payment/callback")]
    public async Task<ActionResult> PaymentCallback()
    {
        try
        {
            var queryParams = Request.Query.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ToString()
            );

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

            var returnUrl =
                _configuration["Payment:VNPay:ReturnUrl"]
                ?? "http://localhost:5173/booking/payment/callback";
            var baseUrl = returnUrl.Replace("/booking/payment/callback", "");

            if (success)
            {
                return Redirect($"{baseUrl}/booking/success");
            }
            else
            {
                return Redirect($"{baseUrl}/booking/failed");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment callback");
            return Redirect("http://localhost:5173/booking/failed");
        }
    }
}
