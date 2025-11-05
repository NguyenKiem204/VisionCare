using DomainOrder = VisionCare.Domain.Entities.Order;
using InfraOrder = VisionCare.Infrastructure.Models.Order;

namespace VisionCare.Infrastructure.Mappings;

public static class OrderMapper
{
    public static DomainOrder ToDomain(InfraOrder model)
    {
        return new DomainOrder
        {
            Id = model.OrderId,
            EncounterId = model.EncounterId,
            OrderType = model.OrderType,
            Name = model.Name,
            Status = model.Status,
            ResultUrl = model.ResultUrl,
            Notes = model.Notes,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static InfraOrder ToInfrastructure(DomainOrder domain)
    {
        return new InfraOrder
        {
            OrderId = domain.Id,
            EncounterId = domain.EncounterId,
            OrderType = domain.OrderType,
            Name = domain.Name,
            Status = domain.Status,
            ResultUrl = domain.ResultUrl,
            Notes = domain.Notes,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt,
        };
    }
}


