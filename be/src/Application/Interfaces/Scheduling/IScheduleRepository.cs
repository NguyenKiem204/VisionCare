using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface IScheduleRepository
{
    Task<IEnumerable<Schedule>> GetAllAsync();
    Task<Schedule?> GetByIdAsync(int id);
    Task<IEnumerable<Schedule>> GetByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Schedule>> GetByDoctorAndDateAsync(int doctorId, DateOnly scheduleDate);
    Task<IEnumerable<Schedule>> GetAvailableSchedulesAsync(
        int doctorId,
        DateOnly scheduleDate,
        int? serviceTypeId
    );
    Task<Schedule?> GetByDoctorSlotAndDateAsync(int doctorId, int slotId, DateOnly scheduleDate);
    Task<Schedule> AddAsync(Schedule schedule);
    Task UpdateAsync(Schedule schedule);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ScheduleExistsAsync(int doctorId, int slotId, DateOnly scheduleDate);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Schedule>> GetPagedAsync(int page, int pageSize);
}
