using VisionCare.Application.DTOs.ServiceDetailDto;

namespace VisionCare.Application.Interfaces.Services;

public interface IServiceDetailService
{
    Task<ServiceDetailDto?> GetServiceDetailByIdAsync(int id);
    Task<ServiceDetailDto?> GetByServiceIdAndTypeIdAsync(int serviceId, int serviceTypeId);
    Task<IEnumerable<ServiceDetailDto>> GetByServiceTypeIdAsync(int serviceTypeId);
}

