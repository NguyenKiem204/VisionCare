using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Services;

public interface IServiceDetailRepository
{
    Task<ServiceDetail?> GetByIdAsync(int id);
    Task<ServiceDetail?> GetByServiceIdAndTypeIdAsync(int serviceId, int serviceTypeId);
    Task<IEnumerable<ServiceDetail>> GetByServiceTypeIdAsync(int serviceTypeId);
}
