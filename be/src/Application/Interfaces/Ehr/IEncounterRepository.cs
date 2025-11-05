using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Ehr;

public interface IEncounterRepository
{
    Task<Encounter?> GetByIdAsync(int id);
    Task<IEnumerable<Encounter>> GetByDoctorAndRangeAsync(
        int doctorId,
        DateOnly? from,
        DateOnly? to
    );
    Task<Encounter> AddAsync(Encounter encounter);
    Task UpdateAsync(Encounter encounter);
}
