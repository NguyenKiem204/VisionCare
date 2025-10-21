using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.MedicalHistoryDto;

public class MedicalHistoryDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public string? Diagnosis { get; set; }
    public string? Symptoms { get; set; }
    public string? Treatment { get; set; }
    public string? Prescription { get; set; }
    public decimal? VisionLeft { get; set; }
    public decimal? VisionRight { get; set; }
    public string? AdditionalTests { get; set; }
    public string? Notes { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorName { get; set; }
    public DateTime? AppointmentDate { get; set; }
}

public class CreateMedicalHistoryRequest
{
    public int AppointmentId { get; set; }
    public string? Diagnosis { get; set; }
    public string? Symptoms { get; set; }
    public string? Treatment { get; set; }
    public string? Prescription { get; set; }
    public decimal? VisionLeft { get; set; }
    public decimal? VisionRight { get; set; }
    public string? AdditionalTests { get; set; }
    public string? Notes { get; set; }
}

public class UpdateMedicalHistoryRequest
{
    public string? Diagnosis { get; set; }
    public string? Symptoms { get; set; }
    public string? Treatment { get; set; }
    public string? Prescription { get; set; }
    public decimal? VisionLeft { get; set; }
    public decimal? VisionRight { get; set; }
    public string? AdditionalTests { get; set; }
    public string? Notes { get; set; }
}

public class MedicalHistorySearchRequest
{
    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Diagnosis { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
