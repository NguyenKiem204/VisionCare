using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class MedicalHistory : BaseEntity
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

    // Navigation properties
    public Appointment Appointment { get; set; } = null!;

    // Domain methods
    public void UpdateDiagnosis(string diagnosis)
    {
        Diagnosis = diagnosis;
        LastModified = DateTime.UtcNow;
    }

    public void AddPrescription(string prescription)
    {
        Prescription = prescription;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateVisionMeasurements(decimal? leftVision, decimal? rightVision)
    {
        if (leftVision.HasValue && (leftVision < 0 || leftVision > 2.0m))
            throw new ArgumentException("Left vision must be between 0.0 and 2.0");

        if (rightVision.HasValue && (rightVision < 0 || rightVision > 2.0m))
            throw new ArgumentException("Right vision must be between 0.0 and 2.0");

        VisionLeft = leftVision;
        VisionRight = rightVision;
        LastModified = DateTime.UtcNow;
    }

    public void AddTreatment(string treatment)
    {
        Treatment = treatment;
        LastModified = DateTime.UtcNow;
    }

    public void AddNotes(string notes)
    {
        Notes = notes;
        LastModified = DateTime.UtcNow;
    }
}
