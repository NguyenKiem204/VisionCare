using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Appointment : BaseEntity
{
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentStatus { get; set; }
    public int? DoctorId { get; set; }
    public int? PatientId { get; set; }
    public string? Notes { get; set; }
    public string? AppointmentCode { get; set; }
    public string? PaymentStatus { get; set; }
    public decimal? ActualCost { get; set; }
    public int? ServiceDetailId { get; set; }
    public int? DiscountId { get; set; }

    // Navigation properties
    public Doctor? Doctor { get; set; }
    public Customer? Patient { get; set; }
    public ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();

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
