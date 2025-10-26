using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Certificates;

public interface ICertificateRepository
{
    Task<IEnumerable<Certificate>> GetAllAsync();
    Task<Certificate?> GetByIdAsync(int id);
    Task<Certificate> AddAsync(Certificate certificate);
    Task UpdateAsync(Certificate certificate);
    Task DeleteAsync(int id);
}
