using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class FollowUp : BaseEntity
{
    public int AppointmentId { get; set; }
    public DateTime? NextAppointmentDate { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "Pending";

    // Navigation properties
    public Appointment Appointment { get; set; } = null!;

    // Domain methods
    public void ScheduleFollowUp(DateTime nextAppointmentDate, string? description = null)
    {
        if (nextAppointmentDate <= DateTime.UtcNow)
            throw new ArgumentException("Follow-up date must be in the future");

        NextAppointmentDate = nextAppointmentDate;
        Description = description;
        Status = "Scheduled";
        LastModified = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = "Completed";
        LastModified = DateTime.UtcNow;
    }

    public void Cancel(string? reason = null)
    {
        Status = "Cancelled";
        Description = reason;
        LastModified = DateTime.UtcNow;
    }

    public void Reschedule(DateTime newDate)
    {
        if (newDate <= DateTime.UtcNow)
            throw new ArgumentException("New follow-up date must be in the future");

        NextAppointmentDate = newDate;
        Status = "Rescheduled";
        LastModified = DateTime.UtcNow;
    }
}
