using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Slot : BaseEntity
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int ServiceTypeId { get; set; }

    // Navigation properties
    public ServiceType ServiceType { get; set; } = null!;
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    // Domain methods
    public void UpdateTime(TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time");

        StartTime = startTime;
        EndTime = endTime;
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
}
