using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Service : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Benefits { get; set; }
    public string Status { get; set; } = "Active";
    public int? SpecializationId { get; set; }

    // Navigation properties
    public Specialization? Specialization { get; set; }
    public ICollection<ServiceDetail> ServiceDetails { get; set; } = new List<ServiceDetail>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    // Domain methods
    public void UpdateName(string newName)
    {
        Name = newName;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
        LastModified = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = "Active";
        LastModified = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = "Inactive";
        LastModified = DateTime.UtcNow;
    }

    public void AssignToSpecialization(int specializationId)
    {
        SpecializationId = specializationId;
        LastModified = DateTime.UtcNow;
    }
}
