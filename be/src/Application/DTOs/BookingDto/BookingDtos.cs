using VisionCare.Domain.Enums;

namespace VisionCare.Application.DTOs.BookingDto;

public class AvailableSlotDto
{
    public int SlotId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int DurationMinutes { get; set; }
    public int AvailableCount { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsOnHold { get; set; }
    public int? HoldByCustomerId { get; set; } // Customer ID đang giữ slot này (nếu có)
}

public class HoldSlotRequest
{
    public int DoctorId { get; set; }
    public int SlotId { get; set; }
    public DateOnly ScheduleDate { get; set; }
    public int? CustomerId { get; set; }
}

public class HoldSlotResponse
{
    public string HoldToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

public class CreateBookingRequest
{
    public string? HoldToken { get; set; } // Required nếu có hold
    public int DoctorId { get; set; }
    public int ServiceDetailId { get; set; }
    public int SlotId { get; set; }
    public DateOnly ScheduleDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public int? CustomerId { get; set; } // Required nếu đã login
    // Nếu chưa login:
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }
    public int? DiscountId { get; set; }
}

public class BookingResponse
{
    public int AppointmentId { get; set; }
    public string AppointmentCode { get; set; } = string.Empty;
    public PaymentStatus PaymentStatus { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? PaymentUrl { get; set; } // Nếu cần thanh toán online
}

public class CancelBookingRequest
{
    public string? Reason { get; set; }
    public bool RequestRefund { get; set; }
}

public class BookingSearchRequest
{
    public string? AppointmentCode { get; set; }
    public string? Phone { get; set; }
    public int? CustomerId { get; set; }
}

public class BookingDto
{
    public int Id { get; set; }
    public string AppointmentCode { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public int PatientId { get; set; }
    public string? PatientName { get; set; }
    public int ServiceDetailId { get; set; }
    public string? ServiceName { get; set; }
    public decimal? ActualCost { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
