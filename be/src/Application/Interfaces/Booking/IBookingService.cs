using VisionCare.Application.DTOs.BookingDto;

namespace VisionCare.Application.Interfaces.Booking;

public interface IBookingService
{
    // Slot Availability
    Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(
        int doctorId,
        DateOnly date,
        int? serviceTypeId = null
    );

    // Hold Management
    Task<HoldSlotResponse> HoldSlotAsync(HoldSlotRequest request);
    Task<bool> ValidateHoldAsync(string holdToken);
    Task ReleaseHoldAsync(string holdToken);

    // Booking Creation
    Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request, string ipAddress);
    Task<bool> CheckDuplicateBookingAsync(CreateBookingRequest request); // 30s window

    // Booking Management
    Task<BookingDto?> GetBookingByIdAsync(int appointmentId);
    Task<BookingDto?> GetBookingByCodeAsync(string appointmentCode);
    Task<BookingDto?> GetBookingByPhoneAsync(string phone);
    Task<bool> CancelBookingAsync(int appointmentId, CancelBookingRequest request);

    // Payment
    Task<string> InitiatePaymentAsync(int appointmentId, string ipAddress);
    Task<bool> ProcessPaymentCallbackAsync(Dictionary<string, string> queryParams);
}