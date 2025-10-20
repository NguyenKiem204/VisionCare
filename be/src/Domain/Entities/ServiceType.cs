using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class ServiceType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }

    // Navigation properties
    public ICollection<ServiceDetail> ServiceDetails { get; set; } = new List<ServiceDetail>();
    public ICollection<Slot> Slots { get; set; } = new List<Slot>();

    // Domain methods
    public void UpdateName(string newName)
    {
        Name = newName;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateDuration(int durationMinutes)
    {
        if (durationMinutes <= 0)
            throw new ArgumentException("Duration must be greater than 0");

        DurationMinutes = durationMinutes;
        LastModified = DateTime.UtcNow;
    }
}
