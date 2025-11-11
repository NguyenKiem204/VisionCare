using VisionCare.Domain.Entities;
using InfrastructureDoctorSchedule = VisionCare.Infrastructure.Models.Doctorschedule;

namespace VisionCare.Infrastructure.Mappings;

public static class DoctorScheduleMapper
{
    public static DoctorSchedule ToDomain(InfrastructureDoctorSchedule model)
    {
        var domain = new DoctorSchedule
        {
            Id = model.DoctorScheduleId,
            DoctorId = model.DoctorId,
            ShiftId = model.ShiftId,
            RoomId = model.RoomId,
            EquipmentId = model.EquipmentId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            DayOfWeek = model.DayOfWeek,
            RecurrenceRule = model.RecurrenceRule ?? "WEEKLY",
            IsActive = model.IsActive ?? true,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = model.UpdatedAt
        };

        // Populate minimal navigation objects so higher layers can expose names/times
        if (model.Doctor != null)
        {
            domain.Doctor = new Doctor
            {
                AccountId = model.Doctor.AccountId,
                DoctorName = model.Doctor.FullName
            };
        }

        if (model.Shift != null)
        {
            domain.Shift = new WorkShift
            {
                Id = model.Shift.ShiftId,
                ShiftName = model.Shift.ShiftName,
                StartTime = model.Shift.StartTime,
                EndTime = model.Shift.EndTime,
                IsActive = model.Shift.IsActive ?? true
            };
        }

        if (model.Room != null)
        {
            domain.Room = new Room
            {
                Id = model.Room.RoomId,
                RoomName = model.Room.RoomName
            };
        }

        if (model.Equipment != null)
        {
            domain.Equipment = new Equipment
            {
                Id = model.Equipment.EquipmentId,
                Name = model.Equipment.Name
            };
        }

        return domain;
    }

    public static InfrastructureDoctorSchedule ToInfrastructure(DoctorSchedule domain)
    {
        return new InfrastructureDoctorSchedule
        {
            DoctorScheduleId = domain.Id,
            DoctorId = domain.DoctorId,
            ShiftId = domain.ShiftId,
            RoomId = domain.RoomId,
            EquipmentId = domain.EquipmentId,
            StartDate = domain.StartDate,
            EndDate = domain.EndDate,
            DayOfWeek = domain.DayOfWeek,
            RecurrenceRule = domain.RecurrenceRule,
            IsActive = domain.IsActive,
            CreatedAt = domain.Created.Kind == DateTimeKind.Unspecified
                ? domain.Created
                : DateTime.SpecifyKind(domain.Created, DateTimeKind.Unspecified),
            UpdatedAt = domain.LastModified.HasValue
                ? (domain.LastModified.Value.Kind == DateTimeKind.Unspecified
                    ? domain.LastModified.Value
                    : DateTime.SpecifyKind(domain.LastModified.Value, DateTimeKind.Unspecified))
                : (DateTime?)null
        };
    }
}

