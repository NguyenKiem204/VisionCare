namespace VisionCare.Application.DTOs.CustomerHistory;

public class CustomerPrescriptionHistoryDto
{
    public int PrescriptionId { get; set; }
    public int EncounterId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Notes { get; set; }
    public string EncounterStatus { get; set; } = string.Empty;
    public DateTime EncounterDate { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public int AppointmentId { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string? DoctorAvatar { get; set; }
    public string? DoctorSpecialization { get; set; }
    public IEnumerable<CustomerPrescriptionLineDto> Lines { get; set; } =
        Array.Empty<CustomerPrescriptionLineDto>();
}

public class CustomerPrescriptionLineDto
{
    public int LineId { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string? DrugCode { get; set; }
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instructions { get; set; }
}

