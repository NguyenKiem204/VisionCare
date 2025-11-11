using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

/// <summary>
/// WorkShift entity - represents a work shift (e.g., Morning, Afternoon, Evening)
/// </summary>
public class WorkShift : BaseEntity
{
    public string ShiftName { get; set; } = string.Empty;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    // Domain methods
    public void UpdateName(string shiftName)
    {
        if (string.IsNullOrWhiteSpace(shiftName))
            throw new ArgumentException("Shift name cannot be empty");

        ShiftName = shiftName;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateTime(TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time");

        StartTime = startTime;
        EndTime = endTime;
        LastModified = DateTime.UtcNow;
    }

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

    public bool IsTimeValid()
    {
        return StartTime < EndTime;
    }

    public int GetDurationMinutes()
    {
        return (int)(EndTime - StartTime).TotalMinutes;
    }

    public bool Overlaps(WorkShift other)
    {
        return StartTime < other.EndTime && EndTime > other.StartTime;
    }
}
