using VisionCare.Application.DTOs.DegreeDto;

namespace VisionCare.Application.Interfaces.Degrees;

public interface IDegreeService
{
    Task<IEnumerable<DegreeDto>> GetAllDegreesAsync();
    Task<DegreeDto?> GetDegreeByIdAsync(int id);
    Task<DegreeDto> CreateDegreeAsync(CreateDegreeRequest request);
    Task<DegreeDto> UpdateDegreeAsync(int id, UpdateDegreeRequest request);
    Task<bool> DeleteDegreeAsync(int id);
}
