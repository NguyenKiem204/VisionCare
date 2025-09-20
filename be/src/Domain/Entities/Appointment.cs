using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Appointment : BaseEntity
{
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentStatus { get; set; }
    public int? DoctorId { get; set; }
    public int? PatientId { get; set; }
    public Doctor? Doctor { get; set; }
    public Customer? Patient { get; set; }
}
