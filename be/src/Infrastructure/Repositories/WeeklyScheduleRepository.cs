using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class WeeklyScheduleRepository : IWeeklyScheduleRepository
{
    private readonly VisionCareDbContext _context;

    public WeeklyScheduleRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WeeklySchedule>> GetByDoctorIdAsync(int doctorId)
    {
        var schedules = await _context
            .Weeklyschedules.Include(ws => ws.Doctor)
            .Include(ws => ws.Slot)
            .Where(ws => ws.DoctorId == doctorId)
            .ToListAsync();

        return schedules.Select(WeeklyScheduleMapper.ToDomain).ToList();
    }

    public async Task<WeeklySchedule?> GetByIdAsync(int id)
    {
        var schedule = await _context
            .Weeklyschedules.Include(ws => ws.Doctor)
            .Include(ws => ws.Slot)
            .FirstOrDefaultAsync(ws => ws.WeeklyScheduleId == id);

        return schedule != null ? WeeklyScheduleMapper.ToDomain(schedule) : null;
    }

    public async Task<WeeklySchedule?> GetByDoctorDayAndSlotAsync(
        int doctorId,
        DayOfWeek dayOfWeek,
        int slotId
    )
    {
        var schedule = await _context
            .Weeklyschedules.Include(ws => ws.Doctor)
            .Include(ws => ws.Slot)
            .FirstOrDefaultAsync(ws =>
                ws.DoctorId == doctorId
                && ws.DayOfWeek == (int)dayOfWeek
                && ws.SlotId == slotId
            );

        return schedule != null ? WeeklyScheduleMapper.ToDomain(schedule) : null;
    }

    public async Task<WeeklySchedule> AddAsync(WeeklySchedule weeklySchedule)
    {
        var model = WeeklyScheduleMapper.ToInfrastructure(weeklySchedule);
        _context.Weeklyschedules.Add(model);
        await _context.SaveChangesAsync();

        // Reload to get navigation properties
        return WeeklyScheduleMapper.ToDomain(
            await _context
                .Weeklyschedules.Include(ws => ws.Doctor)
                .Include(ws => ws.Slot)
                .FirstOrDefaultAsync(ws => ws.WeeklyScheduleId == model.WeeklyScheduleId) ?? model
        );
    }

    public async Task UpdateAsync(WeeklySchedule weeklySchedule)
    {
        var existing = await _context
            .Weeklyschedules.FindAsync(weeklySchedule.Id);
        if (existing == null)
            return;

        existing.DoctorId = weeklySchedule.DoctorId;
        existing.DayOfWeek = (int)weeklySchedule.DayOfWeek;
        existing.SlotId = weeklySchedule.SlotId;
        existing.IsActive = weeklySchedule.IsActive;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var schedule = await _context.Weeklyschedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Weeklyschedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Weeklyschedules.AnyAsync(ws => ws.WeeklyScheduleId == id);
    }

    public async Task<IEnumerable<WeeklySchedule>> GetActiveByDoctorIdAsync(int doctorId)
    {
        var schedules = await _context
            .Weeklyschedules.Include(ws => ws.Doctor)
            .Include(ws => ws.Slot)
            .Where(ws => ws.DoctorId == doctorId && (ws.IsActive == true || ws.IsActive == null))
            .ToListAsync();

        return schedules.Select(WeeklyScheduleMapper.ToDomain).ToList();
    }
}

