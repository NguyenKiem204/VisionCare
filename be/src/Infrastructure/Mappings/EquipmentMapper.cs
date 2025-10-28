using VisionCare.Domain.Entities;
using InfrastructureEquipment = VisionCare.Infrastructure.Models.Equipment;

namespace VisionCare.Infrastructure.Mappings;

public static class EquipmentMapper
{
    public static Equipment ToDomain(InfrastructureEquipment model)
    {
        return new Equipment
        {
            Id = model.EquipmentId,
            Name = model.Name,
            Model = model.Model ?? string.Empty,
            SerialNumber = model.SerialNumber ?? string.Empty,
            Manufacturer = model.Manufacturer ?? string.Empty,
            PurchaseDate = model.PurchaseDate,
            LastMaintenanceDate = model.LastMaintenanceDate,
            Status = model.Status ?? "Active",
            Location = model.Location ?? string.Empty,
            Notes = model.Notes ?? string.Empty,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = model.UpdatedAt
        };
    }

    public static InfrastructureEquipment ToInfrastructure(Equipment domain)
    {
        return new InfrastructureEquipment
        {
            EquipmentId = domain.Id,
            Name = domain.Name,
            Model = domain.Model,
            SerialNumber = domain.SerialNumber,
            Manufacturer = domain.Manufacturer,
            PurchaseDate = domain.PurchaseDate,
            LastMaintenanceDate = domain.LastMaintenanceDate,
            Status = domain.Status,
            Location = domain.Location,
            Notes = domain.Notes,
            CreatedAt = domain.Created,
            UpdatedAt = domain.LastModified
        };
    }
}
