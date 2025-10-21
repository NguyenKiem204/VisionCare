using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces;

public interface ISpecializationRepository
{
    Task<IEnumerable<Specialization>> GetAllAsync();
    Task<Specialization?> GetByIdAsync(int id);
    Task<Specialization?> GetByNameAsync(string name);
    Task<Specialization> AddAsync(Specialization specialization);
    Task UpdateAsync(Specialization specialization);
    Task DeleteAsync(int id);

    // Additional operations for SpecializationManager
    Task<IEnumerable<Specialization>> GetActiveAsync();
    Task<IEnumerable<Specialization>> SearchAsync(string keyword);
    Task<int> GetTotalCountAsync();
    Task<int> GetActiveCountAsync();
    Task<int> GetDoctorsCountBySpecializationAsync(int specializationId);
    Task<Dictionary<string, int>> GetUsageStatsAsync();
}
