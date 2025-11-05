using VisionCare.Domain.Entities;
using InfrastructureRoom = VisionCare.Infrastructure.Models.Room;

namespace VisionCare.Infrastructure.Mappings;

public static class RoomMapper
{
    public static Room ToDomain(InfrastructureRoom model)
    {
        return new Room
        {
            Id = model.RoomId,
            RoomName = model.RoomName,
            RoomCode = model.RoomCode,
            Capacity = model.Capacity ?? 1,
            Status = model.Status ?? "Active",
            Location = model.Location,
            Notes = model.Notes,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = model.UpdatedAt
        };
    }

    public static InfrastructureRoom ToInfrastructure(Room domain)
    {
        return new InfrastructureRoom
        {
            RoomId = domain.Id,
            RoomName = domain.RoomName,
            RoomCode = domain.RoomCode,
            Capacity = domain.Capacity,
            Status = domain.Status,
            Location = domain.Location,
            Notes = domain.Notes,
            CreatedAt = domain.Created,
            UpdatedAt = domain.LastModified
        };
    }
}

