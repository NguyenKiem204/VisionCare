using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.ServiceTypes;

public interface IServiceTypeRepository
{
    Task<IEnumerable<ServiceType>> GetAllAsync();
    Task<ServiceType?> GetByIdAsync(int id);
    Task<ServiceType> AddAsync(ServiceType serviceType);
    Task UpdateAsync(ServiceType serviceType);
    Task DeleteAsync(int id);
    Task<(IEnumerable<ServiceType> items, int totalCount)> SearchAsync(
        string? keyword,
        int? minDuration,
        int? maxDuration,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    );
}
