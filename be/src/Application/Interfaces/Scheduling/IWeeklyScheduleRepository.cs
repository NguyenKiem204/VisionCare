using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface IWeeklyScheduleRepository
{
    Task<IEnumerable<WeeklySchedule>> GetByDoctorIdAsync(int doctorId);
    Task<WeeklySchedule?> GetByIdAsync(int id);
    Task<WeeklySchedule?> GetByDoctorDayAndSlotAsync(int doctorId, DayOfWeek dayOfWeek, int slotId);
    Task<WeeklySchedule> AddAsync(WeeklySchedule weeklySchedule);
    Task UpdateAsync(WeeklySchedule weeklySchedule);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<WeeklySchedule>> GetActiveByDoctorIdAsync(int doctorId);
}
