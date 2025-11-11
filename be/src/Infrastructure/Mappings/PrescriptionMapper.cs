using DomainPrescription = VisionCare.Domain.Entities.Prescription;
using DomainPrescriptionLine = VisionCare.Domain.Entities.PrescriptionLine;
using InfraPrescription = VisionCare.Infrastructure.Models.Prescription;
using InfraPrescriptionLine = VisionCare.Infrastructure.Models.Prescriptionline;

namespace VisionCare.Infrastructure.Mappings;

public static class PrescriptionMapper
{
    public static DomainPrescription ToDomain(InfraPrescription model)
    {
        return new DomainPrescription
        {
            Id = model.PrescriptionId,
            EncounterId = model.EncounterId,
            Notes = model.Notes,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            Lines = model.Prescriptionlines.Select(ToDomain).ToList(),
        };
    }

    public static DomainPrescriptionLine ToDomain(InfraPrescriptionLine model)
    {
        return new DomainPrescriptionLine
        {
            Id = model.LineId,
            PrescriptionId = model.PrescriptionId,
            DrugCode = model.DrugCode,
            DrugName = model.DrugName,
            Dosage = model.Dosage,
            Frequency = model.Frequency,
            Duration = model.Duration,
            Instructions = model.Instructions,
        };
    }

    public static InfraPrescription ToInfrastructure(DomainPrescription domain)
    {
        return new InfraPrescription
        {
            PrescriptionId = domain.Id,
            EncounterId = domain.EncounterId,
            Notes = domain.Notes,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt,
        };
    }

    public static InfraPrescriptionLine ToInfrastructure(DomainPrescriptionLine domain, int prescriptionId)
    {
        return new InfraPrescriptionLine
        {
            PrescriptionId = prescriptionId,
            DrugCode = domain.DrugCode,
            DrugName = domain.DrugName,
            Dosage = domain.Dosage,
            Frequency = domain.Frequency,
            Duration = domain.Duration,
            Instructions = domain.Instructions,
        };
    }
}


