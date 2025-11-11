using VisionCare.Application.DTOs.Ehr;

namespace VisionCare.Application.Interfaces.Ehr;

public interface IPrescriptionService
{
    Task<PrescriptionDto> CreateAsync(CreatePrescriptionRequest request, int doctorId);
    Task<IEnumerable<PrescriptionDto>> GetByEncounterAsync(int encounterId, int doctorId);
}
