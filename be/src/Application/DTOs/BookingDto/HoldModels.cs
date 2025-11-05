namespace VisionCare.Application.DTOs.BookingDto;

/// <summary>
/// Model để serialize/deserialize BookingHold từ Redis cache
/// </summary>
public class BookingHoldData
{
    public string HoldToken { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public int SlotId { get; set; }
    public DateOnly ScheduleDate { get; set; }
    public int? CustomerId { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;
}

public class ReleaseHoldRequest
{
    public string HoldToken { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public int SlotId { get; set; }
    public DateOnly ScheduleDate { get; set; }
}