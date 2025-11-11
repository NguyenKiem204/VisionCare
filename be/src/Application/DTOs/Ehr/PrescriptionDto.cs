namespace VisionCare.Application.DTOs.Ehr;

public class PrescriptionDto
{
    public int Id { get; set; }
    public int EncounterId { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<PrescriptionLineDto> Lines { get; set; } = new();
}

public class PrescriptionLineDto
{
    public int Id { get; set; }
    public int PrescriptionId { get; set; }
    public string? DrugCode { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instructions { get; set; }
}

public class CreatePrescriptionRequest
{
    public int EncounterId { get; set; }
    public string? Notes { get; set; }
    public List<CreatePrescriptionLine> Lines { get; set; } = new();
}

public class CreatePrescriptionLine
{
    public string? DrugCode { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instructions { get; set; }
}


