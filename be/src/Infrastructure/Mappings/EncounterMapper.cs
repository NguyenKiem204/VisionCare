using DomainEncounter = VisionCare.Domain.Entities.Encounter;
using InfraEncounter = VisionCare.Infrastructure.Models.Encounter;

namespace VisionCare.Infrastructure.Mappings;

public static class EncounterMapper
{
    public static DomainEncounter ToDomain(InfraEncounter model)
    {
        return new DomainEncounter
        {
            Id = model.EncounterId,
            AppointmentId = model.AppointmentId,
            DoctorId = model.DoctorId,
            CustomerId = model.CustomerId,
            Subjective = model.Subjective,
            Objective = model.Objective,
            Assessment = model.Assessment,
            Plan = model.Plan,
            Status = model.Status,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static InfraEncounter ToInfrastructure(DomainEncounter domain)
    {
        return new InfraEncounter
        {
            EncounterId = domain.Id,
            AppointmentId = domain.AppointmentId,
            DoctorId = domain.DoctorId,
            CustomerId = domain.CustomerId,
            Subjective = domain.Subjective,
            Objective = domain.Objective,
            Assessment = domain.Assessment,
            Plan = domain.Plan,
            Status = domain.Status,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt,
        };
    }
}


