using VisionCare.Application.DTOs.ServiceDto;

namespace VisionCare.Application.Interfaces.Services;

public interface IServiceService
{
    Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
    Task<ServiceDto?> GetServiceByIdAsync(int id);
    Task<ServiceDto?> GetServiceByNameAsync(string name);
    Task<IEnumerable<ServiceDto>> GetServicesBySpecializationAsync(int specializationId);
    Task<IEnumerable<ServiceDto>> GetActiveServicesAsync();
    Task<ServiceDto> CreateServiceAsync(CreateServiceRequest request);
    Task<ServiceDto> UpdateServiceAsync(int id, UpdateServiceRequest request);
    Task<bool> DeleteServiceAsync(int id);
    Task<ServiceDto> ActivateServiceAsync(int id);
    Task<ServiceDto> DeactivateServiceAsync(int id);
    Task<IEnumerable<ServiceDto>> SearchServicesAsync(ServiceSearchRequest request);
    Task<int> GetTotalServicesCountAsync();
    Task<bool> ServiceNameExistsAsync(string name, int? excludeId = null);
}
