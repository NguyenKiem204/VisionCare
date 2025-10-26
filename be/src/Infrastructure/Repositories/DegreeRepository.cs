using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Degrees;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class DegreeRepository : IDegreeRepository
{
    private readonly VisionCareDbContext _context;

    public DegreeRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Degree>> GetAllAsync()
    {
        var degrees = await _context.Degrees.ToListAsync();
        return degrees.Select(DegreeMapper.ToDomain);
    }

    public async Task<Degree?> GetByIdAsync(int id)
    {
        var degree = await _context.Degrees.FirstOrDefaultAsync(d => d.DegreeId == id);
        return degree != null ? DegreeMapper.ToDomain(degree) : null;
    }

    public async Task<Degree> AddAsync(Degree degree)
    {
        var degreeModel = DegreeMapper.ToInfrastructure(degree);
        _context.Degrees.Add(degreeModel);
        await _context.SaveChangesAsync();
        return DegreeMapper.ToDomain(degreeModel);
    }

    public async Task UpdateAsync(Degree degree)
    {
        var existingDegree = await _context.Degrees.FirstOrDefaultAsync(d => d.DegreeId == degree.Id);
        if (existingDegree != null)
        {
            existingDegree.Name = degree.Name;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var degree = await _context.Degrees.FirstOrDefaultAsync(d => d.DegreeId == id);
        if (degree != null)
        {
            _context.Degrees.Remove(degree);
            await _context.SaveChangesAsync();
        }
    }
}
