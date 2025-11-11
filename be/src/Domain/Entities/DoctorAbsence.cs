using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

/// <summary>
/// Represents doctor's absence (leave, emergency, etc.)
/// </summary>
public class DoctorAbsence : BaseEntity
{
    public int DoctorId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string AbsenceType { get; set; } = "Leave"; // Leave, Emergency, Sick, Other
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    public bool IsResolved { get; set; } = false; // Whether appointments have been handled

    // Navigation properties
    public Doctor Doctor { get; set; } = null!;

    // Domain methods
    public bool ContainsDate(DateOnly date)
    {
        return date >= StartDate && date <= EndDate;
    }

    public void Approve()
    {
        Status = "Approved";
        LastModified = DateTime.UtcNow;
    }

    public void Reject()
    {
        Status = "Rejected";
        LastModified = DateTime.UtcNow;
    }

    public void MarkAsResolved()
    {
        IsResolved = true;
        LastModified = DateTime.UtcNow;
    }
}

