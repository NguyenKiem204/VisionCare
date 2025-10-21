using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.FollowUpDto;

public class FollowUpDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public DateTime? NextAppointmentDate { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorName { get; set; }
    public DateTime? OriginalAppointmentDate { get; set; }
}

public class CreateFollowUpRequest
{
    public int AppointmentId { get; set; }
    public DateTime NextAppointmentDate { get; set; }
    public string? Description { get; set; }
}

public class UpdateFollowUpRequest
{
    public DateTime? NextAppointmentDate { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}

public class FollowUpSearchRequest
{
    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
