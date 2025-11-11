using VisionCare.Domain.Entities;
using InfrastructureWorkShift = VisionCare.Infrastructure.Models.Workshift;

namespace VisionCare.Infrastructure.Mappings;

public static class WorkShiftMapper
{
    public static WorkShift ToDomain(InfrastructureWorkShift model)
    {
        return new WorkShift
        {
            Id = model.ShiftId,
            ShiftName = model.ShiftName,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            IsActive = model.IsActive ?? true,
            Description = model.Description,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = model.UpdatedAt
        };
    }

    public static InfrastructureWorkShift ToInfrastructure(WorkShift domain)
    {
        return new InfrastructureWorkShift
        {
            ShiftId = domain.Id,
            ShiftName = domain.ShiftName,
            StartTime = domain.StartTime,
            EndTime = domain.EndTime,
            IsActive = domain.IsActive,
            Description = domain.Description,
            CreatedAt = domain.Created,
            UpdatedAt = domain.LastModified
        };
    }
}

