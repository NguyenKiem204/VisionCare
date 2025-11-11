using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Prescription : BaseEntity
{
    public int EncounterId { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<PrescriptionLine> Lines { get; set; } = new();
}

public class PrescriptionLine
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


