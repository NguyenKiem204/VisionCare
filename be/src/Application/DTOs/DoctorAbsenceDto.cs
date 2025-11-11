namespace VisionCare.Application.DTOs;

public class DoctorAbsenceDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string AbsenceType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsResolved { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateDoctorAbsenceRequest
{
    public int DoctorId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string AbsenceType { get; set; } = "Leave";
    public string Reason { get; set; } = string.Empty;
}

public class UpdateDoctorAbsenceRequest
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? AbsenceType { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
}

public class HandleAbsenceAppointmentsRequest
{
    public bool AutoAssignSubstitute { get; set; } = true;
    public Dictionary<int, int>? ManualSubstituteAssignments { get; set; } // appointmentId -> substituteDoctorId
    public List<int>? AppointmentIdsToCancel { get; set; }
}

