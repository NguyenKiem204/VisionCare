using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetAllAsync();
    Task<Appointment?> GetByIdAsync(int id);
    Task<Appointment?> GetByCodeAsync(string appointmentCode);
    Task<Appointment> AddAsync(Appointment appointment);
    Task UpdateAsync(Appointment appointment);
    Task DeleteAsync(int id);

    Task<IEnumerable<Appointment>> GetByDoctorAsync(int doctorId, DateTime? date = null);
    Task<IEnumerable<Appointment>> GetByCustomerAsync(int customerId, DateTime? date = null);
    Task<IEnumerable<Appointment>> GetByDateAsync(DateTime date);
    Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Appointment>> GetUpcomingAsync(int? doctorId = null, int? customerId = null);
    Task<IEnumerable<Appointment>> GetOverdueAsync();
    Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime dateTime);
    Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date);
    Task<bool> CheckConflictAsync(
        int doctorId,
        DateTime dateTime,
        int? excludeAppointmentId = null
    );

    Task<(IEnumerable<Appointment> items, int totalCount)> SearchAppointmentsAsync(
        string? keyword,
        string? status,
        int? doctorId,
        int? customerId,
        DateTime? startDate,
        DateTime? endDate,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    );

    Task<int> GetTotalCountAsync();
    Task<Dictionary<string, int>> GetStatusStatsAsync();
    Task<Dictionary<string, int>> GetDoctorStatsAsync();
}
