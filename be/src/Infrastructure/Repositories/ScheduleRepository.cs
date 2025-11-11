using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;
using VisionCare.Infrastructure.Models;

namespace VisionCare.Infrastructure.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly VisionCareDbContext _context;

    public ScheduleRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Schedule>> GetAllAsync()
    {
        var schedules = await _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .Include(s => s.Room)
            .Include(s => s.Equipment)
            .ToListAsync();

        return schedules.Select(ScheduleMapper.ToDomain).ToList();
    }

    public async Task<VisionCare.Domain.Entities.Schedule?> GetByIdAsync(int id)
    {
        var schedule = await _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .Include(s => s.Room)
            .Include(s => s.Equipment)
            .FirstOrDefaultAsync(s => s.ScheduleId == id);

        return schedule != null ? ScheduleMapper.ToDomain(schedule) : null;
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Schedule>> GetByDoctorIdAsync(int doctorId)
    {
        var schedules = await _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .Include(s => s.Room)
            .Include(s => s.Equipment)
            .Where(s => s.DoctorId == doctorId)
            .ToListAsync();

        return schedules.Select(ScheduleMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Schedule>> GetByDoctorAndDateAsync(
        int doctorId,
        DateOnly scheduleDate
    )
    {
        var schedules = await _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .Include(s => s.Room)
            .Include(s => s.Equipment)
            .Where(s => s.DoctorId == doctorId && s.ScheduleDate == scheduleDate)
            .ToListAsync();

        return schedules.Select(ScheduleMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Schedule>> GetAvailableSchedulesAsync(
        int doctorId,
        DateOnly scheduleDate,
        int? serviceTypeId
    )
    {
        var query = _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .Include(s => s.Room)
            .Include(s => s.Equipment)
            .Where(s =>
                s.DoctorId == doctorId && s.ScheduleDate == scheduleDate && s.Status == "Available"
            )
            .AsQueryable();

        if (serviceTypeId.HasValue)
        {
            query = query.Where(s => s.Slot.ServiceTypeId == serviceTypeId);
        }

        var schedules = await query.ToListAsync();
        return schedules.Select(ScheduleMapper.ToDomain).ToList();
    }

    public async Task<VisionCare.Domain.Entities.Schedule?> GetByDoctorSlotAndDateAsync(
        int doctorId,
        int slotId,
        DateOnly scheduleDate
    )
    {
        var schedule = await _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .Include(s => s.Room)
            .Include(s => s.Equipment)
            .FirstOrDefaultAsync(s =>
                s.DoctorId == doctorId && s.SlotId == slotId && s.ScheduleDate == scheduleDate
            );

        return schedule != null ? ScheduleMapper.ToDomain(schedule) : null;
    }

    public async Task<VisionCare.Domain.Entities.Schedule> AddAsync(VisionCare.Domain.Entities.Schedule schedule)
    {
        var infrastructureModel = ScheduleMapper.ToInfrastructure(schedule);
        _context.Schedules.Add(infrastructureModel);
        await _context.SaveChangesAsync();

        // Return the domain entity with updated ID
        schedule.Id = infrastructureModel.ScheduleId;
        return schedule;
    }

    public async Task UpdateAsync(VisionCare.Domain.Entities.Schedule schedule)
    {
        var existingModel = await _context.Schedules.FindAsync(schedule.Id);
        if (existingModel != null)
        {
            ScheduleMapper.UpdateInfrastructureModel(existingModel, schedule);
            _context.Schedules.Update(existingModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Schedules.AnyAsync(s => s.ScheduleId == id);
    }

    public async Task<bool> ScheduleExistsAsync(int doctorId, int slotId, DateOnly scheduleDate)
    {
        return await _context.Schedules.AnyAsync(s =>
            s.DoctorId == doctorId && s.SlotId == slotId && s.ScheduleDate == scheduleDate
        );
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Schedules.CountAsync();
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Schedule>> GetPagedAsync(int page, int pageSize)
    {
        var schedules = await _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .Include(s => s.Room)
            .Include(s => s.Equipment)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return schedules.Select(ScheduleMapper.ToDomain).ToList();
    }

    public async Task<bool> IsResourceAvailableAsync(int? roomId, int? equipmentId, int slotId, DateOnly scheduleDate, int? excludeScheduleId = null)
    {
        // If no room or equipment specified, it's available
        if (!roomId.HasValue && !equipmentId.HasValue)
        {
            return true;
        }

        var query = _context.Schedules
            .Where(s =>
                s.ScheduleDate == scheduleDate &&
                s.SlotId == slotId &&
                s.Status == "Available"
            );

        // Check room conflict
        if (roomId.HasValue)
        {
            query = query.Where(s => s.RoomId == roomId.Value);
        }

        // Check equipment conflict
        if (equipmentId.HasValue)
        {
            query = query.Where(s => s.EquipmentId == equipmentId.Value);
        }

        // Exclude current schedule if updating
        if (excludeScheduleId.HasValue)
        {
            query = query.Where(s => s.ScheduleId != excludeScheduleId.Value);
        }

        var conflictExists = await query.AnyAsync();
        return !conflictExists;
    }

    public async Task<int> CleanupOldSchedulesAsync(int daysOld = 90)
    {
        var cutoffDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-daysOld));
        
        var oldSchedules = await _context.Schedules
            .Where(s => s.ScheduleDate < cutoffDate)
            .ToListAsync();

        var count = oldSchedules.Count;
        
        if (count > 0)
        {
            _context.Schedules.RemoveRange(oldSchedules);
            await _context.SaveChangesAsync();
        }

        return count;
    }

    public async Task<int> DeleteByDoctorAndDateRangeAsync(int doctorId, DateOnly startDate, DateOnly endDate)
    {
        var toDelete = await _context.Schedules
            .Where(s => s.DoctorId == doctorId && s.ScheduleDate >= startDate && s.ScheduleDate <= endDate)
            .ToListAsync();

        var count = toDelete.Count;
        if (count > 0)
        {
            _context.Schedules.RemoveRange(toDelete);
            await _context.SaveChangesAsync();
        }
        return count;
    }

}
