using VisionCare.Domain.Entities;
using InfrastructureSchedule = VisionCare.Infrastructure.Models.Schedule;

namespace VisionCare.Infrastructure.Mappings;

public static class ScheduleMapper
{
    public static Schedule ToDomain(InfrastructureSchedule model)
    {
        var domain = new Schedule
        {
            Id = model.ScheduleId,
            DoctorId = model.DoctorId,
            ScheduleDate = model.ScheduleDate,
            SlotId = model.SlotId,
            Status = model.Status ?? "Available",
            RoomId = model.RoomId,
            EquipmentId = model.EquipmentId,
            Created = DateTime.UtcNow, // Schedules don't have created/updated timestamps in DB
            LastModified = DateTime.UtcNow,
        };

        if (model.Doctor != null)
        {
            domain.Doctor = new Doctor
            {
                AccountId = model.Doctor.AccountId,
                DoctorName = model.Doctor.FullName
            };
        }

        if (model.Slot != null)
        {
            domain.Slot = new Slot
            {
                Id = model.Slot.SlotId,
                StartTime = model.Slot.StartTime,
                EndTime = model.Slot.EndTime,
                ServiceTypeId = model.Slot.ServiceTypeId
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

    public static InfrastructureSchedule ToInfrastructure(Schedule domain)
    {
        return new InfrastructureSchedule
        {
            ScheduleId = domain.Id,
            DoctorId = domain.DoctorId,
            ScheduleDate = domain.ScheduleDate,
            SlotId = domain.SlotId,
            Status = domain.Status,
            RoomId = domain.RoomId,
            EquipmentId = domain.EquipmentId,
        };
    }

    public static void UpdateInfrastructureModel(InfrastructureSchedule model, Schedule domain)
    {
        model.Status = domain.Status;
        model.RoomId = domain.RoomId;
        model.EquipmentId = domain.EquipmentId;
    }
}
