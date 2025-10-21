using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Services;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service?> GetByIdAsync(int id);
    Task<Service?> GetByNameAsync(string name);
    Task<IEnumerable<Service>> GetBySpecializationAsync(int specializationId);
    Task<IEnumerable<Service>> GetActiveAsync();
    Task<IEnumerable<Service>> SearchAsync(string keyword, int? specializationId, string? status);
    Task<Service> AddAsync(Service service);
    Task UpdateAsync(Service service);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> NameExistsAsync(string name, int? excludeId = null);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Service>> GetPagedAsync(int page, int pageSize);
}
