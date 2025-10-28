using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Certificates;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class CertificateRepository : ICertificateRepository
{
    private readonly VisionCareDbContext _context;

    public CertificateRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Certificate>> GetAllAsync()
    {
        var certificates = await _context.Certificates.ToListAsync();
        return certificates.Select(CertificateMapper.ToDomain);
    }

    public async Task<Certificate?> GetByIdAsync(int id)
    {
        var certificate = await _context.Certificates.FirstOrDefaultAsync(c => c.CertificateId == id);
        return certificate != null ? CertificateMapper.ToDomain(certificate) : null;
    }

    public async Task<Certificate> AddAsync(Certificate certificate)
    {
        var certificateModel = CertificateMapper.ToInfrastructure(certificate);
        _context.Certificates.Add(certificateModel);
        await _context.SaveChangesAsync();
        return CertificateMapper.ToDomain(certificateModel);
    }

    public async Task UpdateAsync(Certificate certificate)
    {
        var existingCertificate = await _context.Certificates.FirstOrDefaultAsync(c => c.CertificateId == certificate.Id);
        if (existingCertificate != null)
        {
            existingCertificate.Name = certificate.Name;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var certificate = await _context.Certificates.FirstOrDefaultAsync(c => c.CertificateId == id);
        if (certificate != null)
        {
            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();
        }
    }
}
