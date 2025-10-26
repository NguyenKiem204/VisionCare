using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.DoctorDegrees;

public interface IDoctorDegreeRepository
{
    Task<IEnumerable<DoctorDegree>> GetAllAsync();
    Task<IEnumerable<DoctorDegree>> GetByDoctorIdAsync(int doctorId);
    Task<DoctorDegree?> GetByIdAsync(int id);
    Task<DoctorDegree> AddAsync(DoctorDegree doctorDegree);
    Task UpdateAsync(DoctorDegree doctorDegree);
    Task DeleteAsync(int id);
}
