using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class DoctorAbsenceRepository : IDoctorAbsenceRepository
{
    private readonly VisionCareDbContext _context;

    public DoctorAbsenceRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorAbsence>> GetAllAsync()
    {
        var absences = await _context
            .Doctorabsences.Include(da => da.Doctor)
            .ToListAsync();

        return absences.Select(DoctorAbsenceMapper.ToDomain).ToList();
    }

    public async Task<DoctorAbsence?> GetByIdAsync(int id)
    {
        var absence = await _context
            .Doctorabsences.Include(da => da.Doctor)
            .FirstOrDefaultAsync(da => da.AbsenceId == id);

        return absence != null ? DoctorAbsenceMapper.ToDomain(absence) : null;
    }

    public async Task<IEnumerable<DoctorAbsence>> GetByDoctorIdAsync(int doctorId)
    {
        var absences = await _context
            .Doctorabsences.Include(da => da.Doctor)
            .Where(da => da.DoctorId == doctorId)
            .ToListAsync();

        return absences.Select(DoctorAbsenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<DoctorAbsence>> GetPendingAsync()
    {
        var absences = await _context
            .Doctorabsences.Include(da => da.Doctor)
            .Where(da => da.Status == "Pending")
            .ToListAsync();

        return absences.Select(DoctorAbsenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<DoctorAbsence>> GetByDoctorAndDateRangeAsync(
        int doctorId,
        DateOnly startDate,
        DateOnly endDate
    )
    {
        var absences = await _context
            .Doctorabsences.Include(da => da.Doctor)
            .Where(da =>
                da.DoctorId == doctorId
                && da.Status == "Approved"
                && da.EndDate >= startDate
                && da.StartDate <= endDate
            )
            .ToListAsync();

        return absences.Select(DoctorAbsenceMapper.ToDomain).ToList();
    }

    public async Task<IEnumerable<DoctorAbsence>> GetApprovedByDateRangeAsync(
        DateOnly startDate,
        DateOnly endDate
    )
    {
        var absences = await _context
            .Doctorabsences.Include(da => da.Doctor)
            .Where(da =>
                da.Status == "Approved"
                && da.EndDate >= startDate
                && da.StartDate <= endDate
            )
            .ToListAsync();

        return absences.Select(DoctorAbsenceMapper.ToDomain).ToList();
    }

    public async Task<DoctorAbsence> AddAsync(DoctorAbsence absence)
    {
        var model = DoctorAbsenceMapper.ToInfrastructure(absence);
        _context.Doctorabsences.Add(model);
        await _context.SaveChangesAsync();

        // Reload to get navigation properties
        return DoctorAbsenceMapper.ToDomain(
            await _context
                .Doctorabsences.Include(da => da.Doctor)
                .FirstOrDefaultAsync(da => da.AbsenceId == model.AbsenceId) ?? model
        );
    }

    public async Task UpdateAsync(DoctorAbsence absence)
    {
        var existing = await _context.Doctorabsences.FindAsync(absence.Id);
        if (existing == null)
            return;

        existing.DoctorId = absence.DoctorId;
        existing.StartDate = absence.StartDate;
        existing.EndDate = absence.EndDate;
        existing.AbsenceType = absence.AbsenceType;
        existing.Reason = absence.Reason;
        existing.Status = absence.Status;
        existing.IsResolved = absence.IsResolved;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var absence = await _context.Doctorabsences.FindAsync(id);
        if (absence != null)
        {
            _context.Doctorabsences.Remove(absence);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Doctorabsences.AnyAsync(da => da.AbsenceId == id);
    }
}

