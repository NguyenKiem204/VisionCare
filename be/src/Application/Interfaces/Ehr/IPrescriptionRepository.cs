using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Ehr;

public interface IPrescriptionRepository
{
    Task<Prescription> AddAsync(Prescription prescription, IEnumerable<PrescriptionLine> lines);
    Task<IEnumerable<Prescription>> GetByEncounterAsync(int encounterId);
}
