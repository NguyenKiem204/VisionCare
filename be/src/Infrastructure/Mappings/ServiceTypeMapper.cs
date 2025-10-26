using VisionCare.Domain.Entities;
using InfrastructureServiceType = VisionCare.Infrastructure.Models.Servicestype;

namespace VisionCare.Infrastructure.Mappings;

public static class ServiceTypeMapper
{
    public static ServiceType ToDomain(InfrastructureServiceType model)
    {
        return new ServiceType
        {
            Id = model.ServiceTypeId,
            Name = model.Name,
            DurationMinutes = (int)model.DurationMinutes,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = model.UpdatedAt
        };
    }

    public static InfrastructureServiceType ToInfrastructure(ServiceType domain)
    {
        return new InfrastructureServiceType
        {
            ServiceTypeId = domain.Id,
            Name = domain.Name,
            DurationMinutes = (short)domain.DurationMinutes,
            CreatedAt = domain.Created,
            UpdatedAt = domain.LastModified
        };
    }
}
