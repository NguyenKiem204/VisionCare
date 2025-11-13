namespace VisionCare.Application.DTOs.CustomerHistory;

public class CustomerMedicalHistoryDto
{
    public int MedicalHistoryId { get; set; }
    public int AppointmentId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Diagnosis { get; set; }
    public string? Symptoms { get; set; }
    public string? Treatment { get; set; }
    public string? Prescription { get; set; }
    public decimal? VisionLeft { get; set; }
    public decimal? VisionRight { get; set; }
    public string? AdditionalTests { get; set; }
    public string? Notes { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string AppointmentStatus { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string? DoctorAvatar { get; set; }
    public string ServiceName { get; set; } = string.Empty;
}

