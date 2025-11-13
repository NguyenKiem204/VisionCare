using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Ehr;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;
using InfraEncounter = VisionCare.Infrastructure.Models.Encounter;

namespace VisionCare.Infrastructure.Repositories.Ehr;

public class EncounterRepository : IEncounterRepository
{
    private readonly VisionCareDbContext _db;

    public EncounterRepository(VisionCareDbContext db)
    {
        _db = db;
    }

    public async Task<Encounter?> GetByIdAsync(int id)
    {
        var model = await _db.Encounters.FirstOrDefaultAsync(x => x.EncounterId == id);
        return model == null ? null : EncounterMapper.ToDomain(model);
    }

    public async Task<Encounter?> GetByAppointmentIdAsync(int appointmentId)
    {
        var model = await _db.Encounters.FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
        return model == null ? null : EncounterMapper.ToDomain(model);
    }

    public async Task<IEnumerable<Encounter>> GetByDoctorAndRangeAsync(
        int doctorId,
        DateOnly? from,
        DateOnly? to
    )
    {
        var q = _db.Encounters.AsQueryable().Where(e => e.DoctorId == doctorId);
        if (from.HasValue)
        {
            var fromDt = from.Value.ToDateTime(TimeOnly.MinValue);
            q = q.Where(e => e.CreatedAt >= fromDt);
        }
        if (to.HasValue)
        {
            var toDt = to.Value.ToDateTime(new TimeOnly(23, 59, 59));
            q = q.Where(e => e.CreatedAt <= toDt);
        }
        var list = await q.OrderByDescending(e => e.CreatedAt).ToListAsync();
        return list.Select(EncounterMapper.ToDomain).ToList();
    }

    public async Task<Encounter> AddAsync(Encounter encounter)
    {
        var model = EncounterMapper.ToInfrastructure(encounter);
        _db.Encounters.Add(model);
        await _db.SaveChangesAsync();
        return EncounterMapper.ToDomain(model);
    }

    public async Task UpdateAsync(Encounter encounter)
    {
        var existing = await _db.Encounters.FirstOrDefaultAsync(x => x.EncounterId == encounter.Id);
        if (existing != null)
        {
            existing.Subjective = encounter.Subjective;
            existing.Objective = encounter.Objective;
            existing.Assessment = encounter.Assessment;
            existing.Plan = encounter.Plan;
            existing.Status = encounter.Status;
            existing.UpdatedAt = encounter.UpdatedAt;
            await _db.SaveChangesAsync();
        }
    }
}


