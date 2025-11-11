using VisionCare.Application.DTOs.Ehr;

namespace VisionCare.Application.Interfaces.Ehr;

public interface IEncounterService
{
    Task<EncounterDto?> GetByIdAsync(int id);
    Task<IEnumerable<EncounterDto>> GetByDoctorAndRangeAsync(
        int doctorId,
        DateOnly? from,
        DateOnly? to
    );
    Task<EncounterDto> CreateAsync(int doctorId, int customerId, CreateEncounterRequest request);
    Task<EncounterDto> UpdateAsync(int id, int doctorId, UpdateEncounterRequest request);
}
