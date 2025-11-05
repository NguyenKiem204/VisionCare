using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface IDoctorAbsenceRepository
{
    Task<IEnumerable<DoctorAbsence>> GetAllAsync();
    Task<DoctorAbsence?> GetByIdAsync(int id);
    Task<IEnumerable<DoctorAbsence>> GetByDoctorIdAsync(int doctorId);
    Task<IEnumerable<DoctorAbsence>> GetPendingAsync();
    Task<IEnumerable<DoctorAbsence>> GetByDoctorAndDateRangeAsync(
        int doctorId,
        DateOnly startDate,
        DateOnly endDate
    );
    Task<IEnumerable<DoctorAbsence>> GetApprovedByDateRangeAsync(
        DateOnly startDate,
        DateOnly endDate
    );
    Task<DoctorAbsence> AddAsync(DoctorAbsence absence);
    Task UpdateAsync(DoctorAbsence absence);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
