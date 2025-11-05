using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.WorkShifts;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;
using InfrastructureWorkShift = VisionCare.Infrastructure.Models.Workshift;

namespace VisionCare.Infrastructure.Repositories;

public class WorkShiftRepository : IWorkShiftRepository
{
    private readonly VisionCareDbContext _context;

    public WorkShiftRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkShift>> GetAllAsync()
    {
        var shifts = await _context.Workshifts.ToListAsync();
        return shifts.Select(WorkShiftMapper.ToDomain);
    }

    public async Task<WorkShift?> GetByIdAsync(int id)
    {
        var shift = await _context.Workshifts.FirstOrDefaultAsync(s => s.ShiftId == id);
        return shift != null ? WorkShiftMapper.ToDomain(shift) : null;
    }

    public async Task<WorkShift> AddAsync(WorkShift workShift)
    {
        var shiftModel = WorkShiftMapper.ToInfrastructure(workShift);
        _context.Workshifts.Add(shiftModel);
        await _context.SaveChangesAsync();
        workShift.Id = shiftModel.ShiftId;
        return workShift;
    }

    public async Task UpdateAsync(WorkShift workShift)
    {
        var existingShift = await _context.Workshifts.FirstOrDefaultAsync(s => s.ShiftId == workShift.Id);
        if (existingShift != null)
        {
            existingShift.ShiftName = workShift.ShiftName;
            existingShift.StartTime = workShift.StartTime;
            existingShift.EndTime = workShift.EndTime;
            existingShift.IsActive = workShift.IsActive;
            existingShift.Description = workShift.Description;
            existingShift.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var shift = await _context.Workshifts.FindAsync(id);
        if (shift != null)
        {
            _context.Workshifts.Remove(shift);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Workshifts.AnyAsync(s => s.ShiftId == id);
    }

    public async Task<IEnumerable<WorkShift>> GetActiveShiftsAsync()
    {
        var shifts = await _context.Workshifts
            .Where(s => s.IsActive == true)
            .ToListAsync();
        return shifts.Select(WorkShiftMapper.ToDomain);
    }

    public async Task<IEnumerable<WorkShift>> SearchAsync(
        string? keyword,
        bool? isActive,
        int page = 1,
        int pageSize = 10
    )
    {
        var query = _context.Workshifts.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            var loweredKeyword = keyword.Trim().ToLower();
            query = query.Where(s =>
                s.ShiftName.ToLower().Contains(loweredKeyword) ||
                (s.Description != null && s.Description.ToLower().Contains(loweredKeyword))
            );
        }

        if (isActive.HasValue)
        {
            query = query.Where(s => s.IsActive == isActive.Value);
        }

        var shifts = await query
            .OrderBy(s => s.StartTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return shifts.Select(WorkShiftMapper.ToDomain);
    }
}

