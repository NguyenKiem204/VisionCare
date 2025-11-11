namespace VisionCare.Application.Interfaces.Scheduling;

public interface IScheduleGenerationService
{
    Task<int> GenerateSchedulesForDoctorAsync(int doctorId, int daysAhead = 14);
    Task<int> GenerateSchedulesForAllDoctorsAsync(int daysAhead = 14);
    Task<int> GenerateSchedulesForDateRangeAsync(int doctorId, DateOnly startDate, DateOnly endDate);
    
    /// <summary>
    /// Generate schedules from DoctorSchedule (new flexible scheduling)
    /// </summary>
    Task<int> GenerateSchedulesFromDoctorScheduleAsync(int doctorScheduleId, DateOnly? startDate = null, DateOnly? endDate = null);
    
    /// <summary>
    /// Generate schedules from all active DoctorSchedules for a doctor
    /// </summary>
    Task<int> GenerateSchedulesFromAllDoctorSchedulesAsync(int doctorId, int daysAhead = 14);
}

