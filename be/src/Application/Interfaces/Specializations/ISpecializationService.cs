using VisionCare.Application.DTOs.SpecializationDto;

namespace VisionCare.Application.Interfaces.Specializations;

/// <summary>
/// Service interface for specialization management operations
/// </summary>
public interface ISpecializationService
{
    // Basic CRUD operations
    Task<IEnumerable<SpecializationDto>> GetAllSpecializationsAsync();
    Task<SpecializationDto?> GetSpecializationByIdAsync(int id);
    Task<SpecializationDto> CreateSpecializationAsync(CreateSpecializationRequest request);
    Task<SpecializationDto> UpdateSpecializationAsync(int id, UpdateSpecializationRequest request);
    Task<bool> DeleteSpecializationAsync(int id);

    // Business operations
    Task<IEnumerable<SpecializationDto>> GetActiveSpecializationsAsync();
    Task<SpecializationDto> ActivateSpecializationAsync(int specializationId);
    Task<SpecializationDto> DeactivateSpecializationAsync(int specializationId);
    Task<IEnumerable<SpecializationDto>> SearchSpecializationsAsync(string keyword);

    // Statistics
    Task<int> GetTotalSpecializationsCountAsync();
    Task<int> GetActiveSpecializationsCountAsync();
    Task<Dictionary<string, int>> GetSpecializationUsageStatsAsync();
}
