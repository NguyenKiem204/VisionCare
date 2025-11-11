using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;

namespace VisionCare.Infrastructure.Repositories;

public class ServiceDetailRepository : IServiceDetailRepository
{
    private readonly VisionCareDbContext _context;

    public ServiceDetailRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceDetail?> GetByIdAsync(int id)
    {
        var serviceDetail = await _context
            .Servicesdetails.Include(sd => sd.Service)
            .Include(sd => sd.ServiceType)
            .FirstOrDefaultAsync(sd => sd.ServiceDetailId == id);

        if (serviceDetail == null)
            return null;

        return new ServiceDetail
        {
            Id = serviceDetail.ServiceDetailId,
            ServiceId = serviceDetail.ServiceId,
            ServiceTypeId = serviceDetail.ServiceTypeId,
            Cost = serviceDetail.Cost,
            Service =
                serviceDetail.Service != null
                    ? new Service
                    {
                        Id = serviceDetail.Service.ServiceId,
                        Name = serviceDetail.Service.Name ?? string.Empty,
                    }
                    : null!,
            ServiceType =
                serviceDetail.ServiceType != null
                    ? new ServiceType
                    {
                        Id = serviceDetail.ServiceType.ServiceTypeId,
                        Name = serviceDetail.ServiceType.Name ?? string.Empty,
                        DurationMinutes = serviceDetail.ServiceType.DurationMinutes,
                    }
                    : null!,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };
    }

    public async Task<ServiceDetail?> GetByServiceIdAndTypeIdAsync(int serviceId, int serviceTypeId)
    {
        var serviceDetail = await _context
            .Servicesdetails.Include(sd => sd.Service)
            .Include(sd => sd.ServiceType)
            .FirstOrDefaultAsync(sd =>
                sd.ServiceId == serviceId && sd.ServiceTypeId == serviceTypeId
            );

        if (serviceDetail == null)
            return null;

        return new ServiceDetail
        {
            Id = serviceDetail.ServiceDetailId,
            ServiceId = serviceDetail.ServiceId,
            ServiceTypeId = serviceDetail.ServiceTypeId,
            Cost = serviceDetail.Cost,
            Service =
                serviceDetail.Service != null
                    ? new Service
                    {
                        Id = serviceDetail.Service.ServiceId,
                        Name = serviceDetail.Service.Name ?? string.Empty,
                    }
                    : null!,
            ServiceType =
                serviceDetail.ServiceType != null
                    ? new ServiceType
                    {
                        Id = serviceDetail.ServiceType.ServiceTypeId,
                        Name = serviceDetail.ServiceType.Name ?? string.Empty,
                        DurationMinutes = serviceDetail.ServiceType.DurationMinutes,
                    }
                    : null!,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };
    }
}
