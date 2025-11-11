using VisionCare.Application.DTOs;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface IWeeklyScheduleService
{
    Task<IEnumerable<WeeklyScheduleDto>> GetByDoctorIdAsync(int doctorId);
    Task<WeeklyScheduleDto?> GetByIdAsync(int id);
    Task<WeeklyScheduleDto> CreateAsync(CreateWeeklyScheduleRequest request);
    Task<WeeklyScheduleDto> UpdateAsync(int id, UpdateWeeklyScheduleRequest request);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<WeeklyScheduleDto>> GetActiveByDoctorIdAsync(int doctorId);
}

