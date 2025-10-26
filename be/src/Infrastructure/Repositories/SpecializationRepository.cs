using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;

namespace VisionCare.Infrastructure.Repositories;

public class SpecializationRepository : ISpecializationRepository
{
    private readonly VisionCareDbContext _context;

    public SpecializationRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Specialization>> GetAllAsync()
    {
        var specializations = await _context.Specializations.ToListAsync();
        return specializations.Select(MapToDomain);
    }

    public async Task<Specialization?> GetByIdAsync(int id)
    {
        var specialization = await _context.Specializations.FirstOrDefaultAsync(s =>
            s.SpecializationId == id
        );

        return specialization != null ? MapToDomain(specialization) : null;
    }

    public async Task<Specialization?> GetByNameAsync(string name)
    {
        var specialization = await _context.Specializations.FirstOrDefaultAsync(s =>
            s.Name == name
        );

        return specialization != null ? MapToDomain(specialization) : null;
    }

    public async Task<Specialization> AddAsync(Specialization specialization)
    {
        var specializationModel = MapToModel(specialization);
        _context.Specializations.Add(specializationModel);
        await _context.SaveChangesAsync();
        return MapToDomain(specializationModel);
    }

    public async Task UpdateAsync(Specialization specialization)
    {
        var existingSpecialization = await _context.Specializations.FirstOrDefaultAsync(s =>
            s.SpecializationId == specialization.Id
        );

        if (existingSpecialization != null)
        {
            existingSpecialization.Name = specialization.SpecializationName ?? string.Empty;
            existingSpecialization.Status = specialization.SpecializationStatus ?? "Active";

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var specialization = await _context.Specializations.FindAsync(id);
        if (specialization != null)
        {
            _context.Specializations.Remove(specialization);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Specialization>> GetActiveAsync()
    {
        var specializations = await _context
            .Specializations.Where(s => s.Status == "Active")
            .ToListAsync();

        return specializations.Select(MapToDomain);
    }

    public async Task<IEnumerable<Specialization>> SearchAsync(string keyword)
    {
        var specializations = await _context
            .Specializations.Where(s => s.Name.Contains(keyword))
            .ToListAsync();

        return specializations.Select(MapToDomain);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Specializations.CountAsync();
    }

    public async Task<int> GetActiveCountAsync()
    {
        return await _context.Specializations.CountAsync(s => s.Status == "Active");
    }

    public async Task<int> GetDoctorsCountBySpecializationAsync(int specializationId)
    {
        return await _context.Doctors.CountAsync(d => d.SpecializationId == specializationId);
    }

    public async Task<Dictionary<string, int>> GetUsageStatsAsync()
    {
        return await _context
            .Specializations.GroupBy(s => s.Name)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    private static Specialization MapToDomain(VisionCare.Infrastructure.Models.Specialization model)
    {
        return new Specialization
        {
            Id = model.SpecializationId,
            SpecializationName = model.Name,
            SpecializationStatus = model.Status,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };
    }

    private static VisionCare.Infrastructure.Models.Specialization MapToModel(Specialization domain)
    {
        return new VisionCare.Infrastructure.Models.Specialization
        {
            SpecializationId = domain.Id,
            Name = domain.SpecializationName ?? string.Empty,
            Status = domain.SpecializationStatus!
        };
    }
}
