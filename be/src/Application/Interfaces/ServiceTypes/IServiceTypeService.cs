using VisionCare.Application.DTOs.ServiceTypeDto;

namespace VisionCare.Application.Interfaces.ServiceTypes;

public interface IServiceTypeService
{
    Task<IEnumerable<ServiceTypeDto>> GetAllServiceTypesAsync();
    Task<ServiceTypeDto?> GetServiceTypeByIdAsync(int id);
    Task<ServiceTypeDto> CreateServiceTypeAsync(CreateServiceTypeRequest request);
    Task<ServiceTypeDto> UpdateServiceTypeAsync(int id, UpdateServiceTypeRequest request);
    Task<bool> DeleteServiceTypeAsync(int id);
    Task<(IEnumerable<ServiceTypeDto> items, int totalCount)> SearchServiceTypesAsync(
        string? keyword,
        int? minDuration,
        int? maxDuration,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    );
}
