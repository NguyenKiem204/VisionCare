using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface IDoctorScheduleRepository
{
    Task<IEnumerable<DoctorSchedule>> GetAllAsync();
    Task<DoctorSchedule?> GetByIdAsync(int id);
    Task<IEnumerable<DoctorSchedule>> GetByDoctorIdAsync(int doctorId);
    Task<IEnumerable<DoctorSchedule>> GetActiveByDoctorIdAsync(int doctorId);
    Task<IEnumerable<DoctorSchedule>> GetByDateRangeAsync(
        int doctorId,
        DateOnly startDate,
        DateOnly endDate
    );
    Task<DoctorSchedule> AddAsync(DoctorSchedule doctorSchedule);
    Task UpdateAsync(DoctorSchedule doctorSchedule);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsConflictAsync(
        int doctorId,
        int shiftId,
        DateOnly startDate,
        DateOnly? endDate,
        int? dayOfWeek,
        int? excludeId = null
    );
}

