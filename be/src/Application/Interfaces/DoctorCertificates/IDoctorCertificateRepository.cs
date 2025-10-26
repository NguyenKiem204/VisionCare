using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.DoctorCertificates;

public interface IDoctorCertificateRepository
{
    Task<IEnumerable<DoctorCertificate>> GetAllAsync();
    Task<IEnumerable<DoctorCertificate>> GetByDoctorIdAsync(int doctorId);
    Task<DoctorCertificate?> GetByIdAsync(int id);
    Task<DoctorCertificate> AddAsync(DoctorCertificate doctorCertificate);
    Task UpdateAsync(DoctorCertificate doctorCertificate);
    Task DeleteAsync(int id);
}
