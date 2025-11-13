using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.DoctorCertificates;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class DoctorCertificateRepository : IDoctorCertificateRepository
{
    private readonly VisionCareDbContext _context;

    public DoctorCertificateRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorCertificate>> GetAllAsync()
    {
        var doctorCertificates = await _context.Certificatedoctors
            .Include(dc => dc.Doctor)
            .Include(dc => dc.Certificate)
            .ToListAsync();
        return doctorCertificates.Select(DoctorCertificateMapper.ToDomain);
    }

    public async Task<IEnumerable<DoctorCertificate>> GetByDoctorIdAsync(int doctorId)
    {
        var doctorCertificates = await _context.Certificatedoctors
            .Include(dc => dc.Doctor)
            .Include(dc => dc.Certificate)
            .Where(dc => dc.DoctorId == doctorId)
            .ToListAsync();
        return doctorCertificates.Select(DoctorCertificateMapper.ToDomain);
    }

    public Task<DoctorCertificate?> GetByIdAsync(int id)
    {
        // Note: This is a composite key, so we need to handle it differently
        // For now, we'll return null as we need both DoctorId and CertificateId
        return Task.FromResult<DoctorCertificate?>(null);
    }

    public async Task<DoctorCertificate> AddAsync(DoctorCertificate doctorCertificate)
    {
        var doctorCertificateModel = DoctorCertificateMapper.ToInfrastructure(doctorCertificate);
        _context.Certificatedoctors.Add(doctorCertificateModel);
        await _context.SaveChangesAsync();

        await _context.Entry(doctorCertificateModel).Reference(dc => dc.Doctor).LoadAsync();
        await _context.Entry(doctorCertificateModel).Reference(dc => dc.Certificate).LoadAsync();

        return DoctorCertificateMapper.ToDomain(doctorCertificateModel);
    }

    public async Task UpdateAsync(DoctorCertificate doctorCertificate)
    {
        var existingDoctorCertificate = await _context.Certificatedoctors
            .FirstOrDefaultAsync(dc => dc.DoctorId == doctorCertificate.DoctorId && 
                                     dc.CertificateId == doctorCertificate.CertificateId);
        if (existingDoctorCertificate != null)
        {
            existingDoctorCertificate.IssuedDate = doctorCertificate.IssuedDate;
            existingDoctorCertificate.IssuedBy = doctorCertificate.IssuedBy;
            existingDoctorCertificate.CertificateImage = doctorCertificate.CertificateImage;
            existingDoctorCertificate.ExpiryDate = doctorCertificate.ExpiryDate;
            existingDoctorCertificate.Status = doctorCertificate.Status;
            await _context.SaveChangesAsync();
        }
    }

    public Task DeleteAsync(int id)
    {
        // Note: This is a composite key, so we need to handle it differently
        // For now, we'll do nothing as we need both DoctorId and CertificateId
        return Task.CompletedTask;
    }
}
