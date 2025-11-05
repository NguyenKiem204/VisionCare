using VisionCare.Application.DTOs.DoctorScheduleDto;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface IDoctorScheduleService
{
    Task<IEnumerable<DoctorScheduleDto>> GetAllDoctorSchedulesAsync();
    Task<DoctorScheduleDto?> GetDoctorScheduleByIdAsync(int id);
    Task<IEnumerable<DoctorScheduleDto>> GetDoctorSchedulesByDoctorIdAsync(int doctorId);
    Task<IEnumerable<DoctorScheduleDto>> GetActiveDoctorSchedulesByDoctorIdAsync(int doctorId);
    Task<DoctorScheduleDto> CreateDoctorScheduleAsync(CreateDoctorScheduleRequest request);
    Task<DoctorScheduleDto> UpdateDoctorScheduleAsync(int id, UpdateDoctorScheduleRequest request);
    Task<bool> DeleteDoctorScheduleAsync(int id);
    Task<bool> HasConflictAsync(int doctorId, int shiftId, DateOnly startDate, DateOnly? endDate, int? dayOfWeek, int? excludeId = null);
}

