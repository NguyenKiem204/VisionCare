using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Encounter : BaseEntity
{
    public int AppointmentId { get; set; }
    public int DoctorId { get; set; }
    public int CustomerId { get; set; }
    public string? Subjective { get; set; }
    public string? Objective { get; set; }
    public string? Assessment { get; set; }
    public string? Plan { get; set; }
    public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}


