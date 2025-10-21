using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
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
            .ToListAsync();

        return schedules.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<VisionCare.Domain.Entities.Schedule?> GetByIdAsync(int id)
    {
        var schedule = await _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .FirstOrDefaultAsync(s => s.ScheduleId == id);

        return schedule != null ? ConvertToDomainEntity(schedule) : null;
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Schedule>> GetByDoctorIdAsync(int doctorId)
    {
        var schedules = await _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .Where(s => s.DoctorId == doctorId)
            .ToListAsync();

        return schedules.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Schedule>> GetByDoctorAndDateAsync(
        int doctorId,
        DateOnly scheduleDate
    )
    {
        var schedules = await _context
            .Schedules.Include(s => s.Doctor)
            .Include(s => s.Slot)
            .Where(s => s.DoctorId == doctorId && s.ScheduleDate == scheduleDate)
            .ToListAsync();

        return schedules.Select(ConvertToDomainEntity).ToList();
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
            .Where(s =>
                s.DoctorId == doctorId && s.ScheduleDate == scheduleDate && s.Status == "Available"
            )
            .AsQueryable();

        if (serviceTypeId.HasValue)
        {
            query = query.Where(s => s.Slot.ServiceTypeId == serviceTypeId);
        }

        var schedules = await query.ToListAsync();
        return schedules.Select(ConvertToDomainEntity).ToList();
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
            .FirstOrDefaultAsync(s =>
                s.DoctorId == doctorId && s.SlotId == slotId && s.ScheduleDate == scheduleDate
            );

        return schedule != null ? ConvertToDomainEntity(schedule) : null;
    }

    public async Task<VisionCare.Domain.Entities.Schedule> AddAsync(VisionCare.Domain.Entities.Schedule schedule)
    {
        var infrastructureModel = ConvertToInfrastructureModel(schedule);
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
            UpdateInfrastructureModel(existingModel, schedule);
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
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return schedules.Select(ConvertToDomainEntity).ToList();
    }

    private static VisionCare.Domain.Entities.Schedule ConvertToDomainEntity(Infrastructure.Models.Schedule model)
    {
        return new VisionCare.Domain.Entities.Schedule
        {
            Id = model.ScheduleId,
            DoctorId = model.DoctorId,
            ScheduleDate = model.ScheduleDate,
            SlotId = model.SlotId,
            Status = model.Status ?? "Available",
            Created = DateTime.UtcNow, // Schedules don't have created/updated timestamps in DB
            LastModified = DateTime.UtcNow,
        };
    }

    private static Infrastructure.Models.Schedule ConvertToInfrastructureModel(
        VisionCare.Domain.Entities.Schedule domainEntity
    )
    {
        return new Infrastructure.Models.Schedule
        {
            ScheduleId = domainEntity.Id,
            DoctorId = domainEntity.DoctorId,
            ScheduleDate = domainEntity.ScheduleDate,
            SlotId = domainEntity.SlotId,
            Status = domainEntity.Status,
        };
    }

    private static void UpdateInfrastructureModel(
        Infrastructure.Models.Schedule model,
        VisionCare.Domain.Entities.Schedule domainEntity
    )
    {
        model.Status = domainEntity.Status;
    }
}
