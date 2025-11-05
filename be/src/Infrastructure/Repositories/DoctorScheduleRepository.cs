using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;
using InfrastructureDoctorSchedule = VisionCare.Infrastructure.Models.Doctorschedule;

namespace VisionCare.Infrastructure.Repositories;

public class DoctorScheduleRepository : IDoctorScheduleRepository
{
    private readonly VisionCareDbContext _context;

    public DoctorScheduleRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorSchedule>> GetAllAsync()
    {
        var schedules = await _context.Doctorschedules
            .Include(ds => ds.Doctor)
            .Include(ds => ds.Shift)
            .Include(ds => ds.Room)
            .Include(ds => ds.Equipment)
            .ToListAsync();
        return schedules.Select(DoctorScheduleMapper.ToDomain);
    }

    public async Task<DoctorSchedule?> GetByIdAsync(int id)
    {
        var schedule = await _context.Doctorschedules
            .Include(ds => ds.Doctor)
            .Include(ds => ds.Shift)
            .Include(ds => ds.Room)
            .Include(ds => ds.Equipment)
            .FirstOrDefaultAsync(ds => ds.DoctorScheduleId == id);
        return schedule != null ? DoctorScheduleMapper.ToDomain(schedule) : null;
    }

    public async Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId)
    {
        var schedules = await _context.Doctorschedules
            .Include(ds => ds.Doctor)
            .Include(ds => ds.Shift)
            .Include(ds => ds.Room)
            .Include(ds => ds.Equipment)
            .Where(ds => ds.DoctorId == doctorId)
            .ToListAsync();
        return schedules.Select(DoctorScheduleMapper.ToDomain);
    }

    public async Task<IEnumerable<DoctorSchedule>> GetActiveByDoctorIdAsync(int doctorId)
    {
        var schedules = await _context.Doctorschedules
            .Include(ds => ds.Doctor)
            .Include(ds => ds.Shift)
            .Include(ds => ds.Room)
            .Include(ds => ds.Equipment)
            .Where(ds => ds.DoctorId == doctorId && ds.IsActive == true)
            .ToListAsync();
        return schedules.Select(DoctorScheduleMapper.ToDomain);
    }

    public async Task<IEnumerable<DoctorSchedule>> GetByDateRangeAsync(
        int doctorId,
        DateOnly startDate,
        DateOnly endDate
    )
    {
        var schedules = await _context.Doctorschedules
            .Include(ds => ds.Doctor)
            .Include(ds => ds.Shift)
            .Include(ds => ds.Room)
            .Include(ds => ds.Equipment)
            .Where(ds =>
                ds.DoctorId == doctorId
                && ds.IsActive == true
                && ds.StartDate <= endDate
                && (ds.EndDate == null || ds.EndDate >= startDate)
            )
            .ToListAsync();
        return schedules.Select(DoctorScheduleMapper.ToDomain);
    }

    public async Task<DoctorSchedule> AddAsync(DoctorSchedule doctorSchedule)
    {
        var scheduleModel = DoctorScheduleMapper.ToInfrastructure(doctorSchedule);
        _context.Doctorschedules.Add(scheduleModel);
        await _context.SaveChangesAsync();
        doctorSchedule.Id = scheduleModel.DoctorScheduleId;
        return doctorSchedule;
    }

    public async Task UpdateAsync(DoctorSchedule doctorSchedule)
    {
        var existingSchedule = await _context.Doctorschedules
            .FirstOrDefaultAsync(ds => ds.DoctorScheduleId == doctorSchedule.Id);
        if (existingSchedule != null)
        {
            existingSchedule.DoctorId = doctorSchedule.DoctorId;
            existingSchedule.ShiftId = doctorSchedule.ShiftId;
            existingSchedule.RoomId = doctorSchedule.RoomId;
            existingSchedule.EquipmentId = doctorSchedule.EquipmentId;
            existingSchedule.StartDate = doctorSchedule.StartDate;
            existingSchedule.EndDate = doctorSchedule.EndDate;
            existingSchedule.DayOfWeek = doctorSchedule.DayOfWeek;
            existingSchedule.RecurrenceRule = doctorSchedule.RecurrenceRule;
            existingSchedule.IsActive = doctorSchedule.IsActive;
            existingSchedule.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var schedule = await _context.Doctorschedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Doctorschedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Doctorschedules.AnyAsync(ds => ds.DoctorScheduleId == id);
    }

    public async Task<bool> ExistsConflictAsync(
        int doctorId,
        int shiftId,
        DateOnly startDate,
        DateOnly? endDate,
        int? dayOfWeek,
        int? excludeId = null
    )
    {
        var query = _context.Doctorschedules
            .Where(ds =>
                ds.DoctorId == doctorId
                && ds.ShiftId == shiftId
                && ds.IsActive == true
                && ds.StartDate <= (endDate ?? startDate)
                && (ds.EndDate == null || ds.EndDate >= startDate)
            );

        if (dayOfWeek.HasValue)
        {
            query = query.Where(ds => ds.DayOfWeek == null || ds.DayOfWeek == dayOfWeek.Value);
        }

        if (excludeId.HasValue)
        {
            query = query.Where(ds => ds.DoctorScheduleId != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}

