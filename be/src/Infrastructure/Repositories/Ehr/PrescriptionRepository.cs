using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Ehr;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using InfraPrescription = VisionCare.Infrastructure.Models.Prescription;
using InfraPrescriptionLine = VisionCare.Infrastructure.Models.Prescriptionline;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories.Ehr;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly VisionCareDbContext _db;

    public PrescriptionRepository(VisionCareDbContext db)
    {
        _db = db;
    }

    public async Task<Prescription> AddAsync(
        Prescription prescription,
        IEnumerable<PrescriptionLine> lines
    )
    {
        var model = new InfraPrescription
        {
            EncounterId = prescription.EncounterId,
            Notes = prescription.Notes,
            CreatedAt = prescription.CreatedAt,
            UpdatedAt = prescription.UpdatedAt,
        };
        _db.Prescriptions.Add(model);
        await _db.SaveChangesAsync();
        foreach (var l in lines)
        {
            var ln = new InfraPrescriptionLine
            {
                PrescriptionId = model.PrescriptionId,
                DrugCode = l.DrugCode,
                DrugName = l.DrugName,
                Dosage = l.Dosage,
                Frequency = l.Frequency,
                Duration = l.Duration,
                Instructions = l.Instructions,
            };
            _db.Prescriptionlines.Add(ln);
        }
        await _db.SaveChangesAsync();
        var loaded = await _db.Prescriptions
            .Include(p => p.Prescriptionlines)
            .FirstAsync(p => p.PrescriptionId == model.PrescriptionId);
        return PrescriptionMapper.ToDomain(loaded);
    }

    public async Task<IEnumerable<Prescription>> GetByEncounterAsync(int encounterId)
    {
        var list = await _db.Prescriptions
            .Include(p => p.Prescriptionlines)
            .Where(p => p.EncounterId == encounterId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return list.Select(PrescriptionMapper.ToDomain).ToList();
    }
}


