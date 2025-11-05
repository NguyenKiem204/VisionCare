using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

/// <summary>
/// Weekly schedule template for doctors - defines recurring weekly schedule
/// </summary>
public class WeeklySchedule : BaseEntity
{
    public int DoctorId { get; set; }
    public DayOfWeek DayOfWeek { get; set; } // 0=Sunday, 1=Monday, etc.
    public int SlotId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Doctor Doctor { get; set; } = null!;
    public Slot Slot { get; set; } = null!;

    // Domain methods
    public void Activate()
    {
        IsActive = true;
        LastModified = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        LastModified = DateTime.UtcNow;
    }
}

