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
    
    /// <summary>
    /// Check if room/equipment is available for a specific time slot
    /// </summary>
    Task<bool> IsResourceAvailableAsync(int? roomId, int? equipmentId, int slotId, DateOnly scheduleDate, int? excludeScheduleId = null);
    
    /// <summary>
    /// Delete schedules older than specified days
    /// </summary>
    Task<int> CleanupOldSchedulesAsync(int daysOld = 90);

    /// <summary>
    /// Delete all schedules of a doctor within a date range (inclusive)
    /// </summary>
    Task<int> DeleteByDoctorAndDateRangeAsync(int doctorId, DateOnly startDate, DateOnly endDate);
}
