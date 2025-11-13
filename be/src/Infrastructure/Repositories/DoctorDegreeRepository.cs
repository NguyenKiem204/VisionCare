using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.DoctorDegrees;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class DoctorDegreeRepository : IDoctorDegreeRepository
{
    private readonly VisionCareDbContext _context;

    public DoctorDegreeRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorDegree>> GetAllAsync()
    {
        var doctorDegrees = await _context.Degreedoctors
            .Include(dd => dd.Doctor)
            .Include(dd => dd.Degree)
            .ToListAsync();
        return doctorDegrees.Select(DoctorDegreeMapper.ToDomain);
    }

    public async Task<IEnumerable<DoctorDegree>> GetByDoctorIdAsync(int doctorId)
    {
        var doctorDegrees = await _context.Degreedoctors
            .Include(dd => dd.Doctor)
            .Include(dd => dd.Degree)
            .Where(dd => dd.DoctorId == doctorId)
            .ToListAsync();
        return doctorDegrees.Select(DoctorDegreeMapper.ToDomain);
    }

    public Task<DoctorDegree?> GetByIdAsync(int id)
    {
        // Note: This is a composite key, so we need to handle it differently
        // For now, we'll return null as we need both DoctorId and DegreeId
        return Task.FromResult<DoctorDegree?>(null);
    }

    public async Task<DoctorDegree> AddAsync(DoctorDegree doctorDegree)
    {
        var doctorDegreeModel = DoctorDegreeMapper.ToInfrastructure(doctorDegree);
        _context.Degreedoctors.Add(doctorDegreeModel);
        await _context.SaveChangesAsync();

        await _context.Entry(doctorDegreeModel).Reference(dd => dd.Doctor).LoadAsync();
        await _context.Entry(doctorDegreeModel).Reference(dd => dd.Degree).LoadAsync();

        return DoctorDegreeMapper.ToDomain(doctorDegreeModel);
    }

    public async Task UpdateAsync(DoctorDegree doctorDegree)
    {
        var existingDoctorDegree = await _context.Degreedoctors
            .FirstOrDefaultAsync(dd => dd.DoctorId == doctorDegree.DoctorId && 
                                     dd.DegreeId == doctorDegree.DegreeId);
        if (existingDoctorDegree != null)
        {
            existingDoctorDegree.IssuedDate = doctorDegree.IssuedDate;
            existingDoctorDegree.IssuedBy = doctorDegree.IssuedBy;
            existingDoctorDegree.CertificateImage = doctorDegree.CertificateImage;
            existingDoctorDegree.Status = doctorDegree.Status;
            await _context.SaveChangesAsync();
        }
    }

    public Task DeleteAsync(int id)
    {
        // Note: This is a composite key, so we need to handle it differently
        // For now, we'll do nothing as we need both DoctorId and DegreeId
        return Task.CompletedTask;
    }
}
