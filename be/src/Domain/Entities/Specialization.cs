using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Specialization : BaseEntity
{
    public string? SpecializationName { get; set; }
    public string? SpecializationStatus { get; set; }

    // Domain methods
    public void UpdateName(string newName)
    {
        SpecializationName = newName;
        LastModified = DateTime.UtcNow;
    }

    public void Activate()
    {
        SpecializationStatus = "Active";
        LastModified = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        SpecializationStatus = "Inactive";
        LastModified = DateTime.UtcNow;
    }
}
