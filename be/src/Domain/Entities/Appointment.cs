using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Appointment : BaseEntity
{
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentStatus { get; set; }
    public int? DoctorId { get; set; }
    public int? PatientId { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public Doctor? Doctor { get; set; }
    public Customer? Patient { get; set; }

    // Domain methods
    public void Confirm()
    {
        AppointmentStatus = "Confirmed";
        LastModified = DateTime.UtcNow;
    }

    public void Cancel(string? reason = null)
    {
        AppointmentStatus = "Cancelled";
        LastModified = DateTime.UtcNow;
    }

    public void Complete(string? notes = null)
    {
        AppointmentStatus = "Completed";
        Notes = notes ?? Notes;
        LastModified = DateTime.UtcNow;
    }

    public void Reschedule(DateTime newDateTime)
    {
        AppointmentDate = newDateTime;
        AppointmentStatus = "Rescheduled";
        LastModified = DateTime.UtcNow;
    }
}
