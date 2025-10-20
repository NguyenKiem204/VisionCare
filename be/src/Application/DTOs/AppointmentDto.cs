using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.AppointmentDto;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentStatus { get; set; }
    public int? DoctorId { get; set; }
    public int? PatientId { get; set; }
    public string? DoctorName { get; set; }
    public string? PatientName { get; set; }
    public string? SpecializationName { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public string? Notes { get; set; }
}

public class CreateAppointmentRequest
{
    public DateTime AppointmentDate { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string? Notes { get; set; }
}

public class UpdateAppointmentRequest
{
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentStatus { get; set; }
    public string? Notes { get; set; }
}

public class RescheduleAppointmentRequest
{
    public DateTime NewAppointmentDate { get; set; }
    public string? Reason { get; set; }
}

public class AppointmentSearchRequest
{
    public int? DoctorId { get; set; }
    public int? PatientId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
