using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Schedule : BaseEntity
{
    public int DoctorId { get; set; }
    public DateOnly ScheduleDate { get; set; }
    public int SlotId { get; set; }
    public string Status { get; set; } = "Available";

    // Navigation properties
    public Doctor Doctor { get; set; } = null!;
    public Slot Slot { get; set; } = null!;

    // Domain methods
    public void MarkAsBooked()
    {
        Status = "Booked";
        LastModified = DateTime.UtcNow;
    }

    public void MarkAsAvailable()
    {
        Status = "Available";
        LastModified = DateTime.UtcNow;
    }

    public void Block(string? reason = null)
    {
        Status = "Blocked";
        LastModified = DateTime.UtcNow;
    }

    public bool IsAvailable()
    {
        return Status == "Available";
    }

    public bool IsBooked()
    {
        return Status == "Booked";
    }

    public bool IsBlocked()
    {
        return Status == "Blocked";
    }

    public bool IsValidForBooking()
    {
        return IsAvailable() && ScheduleDate >= DateOnly.FromDateTime(DateTime.Today);
    }
}
