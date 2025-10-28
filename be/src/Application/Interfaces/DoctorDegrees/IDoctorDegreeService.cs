using VisionCare.Application.DTOs.DoctorDegreeDto;

namespace VisionCare.Application.Interfaces.DoctorDegrees;

public interface IDoctorDegreeService
{
    Task<IEnumerable<DoctorDegreeDto>> GetAllDoctorDegreesAsync();
    Task<IEnumerable<DoctorDegreeDto>> GetDegreesByDoctorAsync(int doctorId);
    Task<DoctorDegreeDto?> GetDoctorDegreeByIdAsync(int id);
    Task<DoctorDegreeDto> CreateDoctorDegreeAsync(CreateDoctorDegreeRequest request);
    Task<DoctorDegreeDto> UpdateDoctorDegreeAsync(int id, UpdateDoctorDegreeRequest request);
    Task<bool> DeleteDoctorDegreeAsync(int id);
}
