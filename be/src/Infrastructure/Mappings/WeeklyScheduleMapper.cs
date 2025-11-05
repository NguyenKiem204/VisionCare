using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class WeeklyScheduleMapper
{
    public static WeeklySchedule ToDomain(VisionCare.Infrastructure.Models.Weeklyschedule model)
    {
        return new WeeklySchedule
        {
            Id = model.WeeklyScheduleId,
            DoctorId = model.DoctorId,
            DayOfWeek = (DayOfWeek)model.DayOfWeek,
            SlotId = model.SlotId,
            IsActive = model.IsActive ?? true,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = model.UpdatedAt ?? DateTime.UtcNow,
        };
    }

    public static VisionCare.Infrastructure.Models.Weeklyschedule ToInfrastructure(
        WeeklySchedule domain
    )
    {
        return new VisionCare.Infrastructure.Models.Weeklyschedule
        {
            WeeklyScheduleId = domain.Id,
            DoctorId = domain.DoctorId,
            DayOfWeek = (int)domain.DayOfWeek,
            SlotId = domain.SlotId,
            IsActive = domain.IsActive,
            CreatedAt = domain.Created,
            UpdatedAt = domain.LastModified,
        };
    }
}
