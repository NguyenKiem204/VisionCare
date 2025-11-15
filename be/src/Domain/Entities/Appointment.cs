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

    public void RequestReschedule(DateTime proposedDateTime, string requestedBy, string? reason = null)
    {
        AppointmentStatus = "PendingReschedule";
        var reasonText = reason != null ? $". Lý do: {reason}" : "";
        var existingNotes = !string.IsNullOrEmpty(Notes) ? $"{Notes}\n" : "";
        
        // Convert to local time (Vietnam timezone UTC+7) for display in Notes
        // If DateTime is UTC, convert to local; otherwise use as-is
        DateTime localDateTime = proposedDateTime;
        if (proposedDateTime.Kind == DateTimeKind.Utc)
        {
            // Convert UTC to Vietnam timezone (UTC+7)
            localDateTime = proposedDateTime.AddHours(7);
            // Ensure Unspecified kind to avoid timezone issues when formatting
            localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
        }
        else if (proposedDateTime.Kind == DateTimeKind.Local)
        {
            // Convert Local to Unspecified to avoid timezone conversion
            localDateTime = DateTime.SpecifyKind(proposedDateTime, DateTimeKind.Unspecified);
        }
        else if (proposedDateTime.Kind == DateTimeKind.Unspecified)
        {
            // Already in local time, use as-is
            localDateTime = proposedDateTime;
        }
        
        // Format using local time components (day, month, year, hour, minute)
        // This ensures the time displayed matches what user selected
        Notes = $"{existingNotes}[{requestedBy}] Đề xuất đổi lịch: {localDateTime:dd/MM/yyyy HH:mm}{reasonText}";
        LastModified = DateTime.UtcNow;
    }

    public void ApproveReschedule(DateTime newDateTime)
    {
        AppointmentDate = newDateTime;
        AppointmentStatus = "Rescheduled";
        LastModified = DateTime.UtcNow;
    }

    public void RejectReschedule(string? reason = null)
    {
        AppointmentStatus = "Confirmed";
        if (reason != null)
        {
            Notes = $"{Notes}\n[Từ chối đổi lịch] Lý do: {reason}";
        }
        LastModified = DateTime.UtcNow;
    }
}
