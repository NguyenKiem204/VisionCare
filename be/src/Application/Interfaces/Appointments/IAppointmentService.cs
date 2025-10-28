using VisionCare.Application.DTOs.AppointmentDto;

namespace VisionCare.Application.Interfaces.Appointments;

/// <summary>
/// Service interface for appointment management operations
/// </summary>
public interface IAppointmentService
{
    // Basic CRUD operations
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
    Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentRequest request);
    Task<AppointmentDto> UpdateAppointmentAsync(int id, UpdateAppointmentRequest request);
    Task<bool> DeleteAppointmentAsync(int id);

    // Scheduling operations
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(
        int doctorId,
        DateTime? date = null
    );
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByCustomerAsync(
        int customerId,
        DateTime? date = null
    );
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateAsync(DateTime date);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateRangeAsync(
        DateTime startDate,
        DateTime endDate
    );
    Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(
        int? doctorId = null,
        int? customerId = null
    );

    // Status management
    Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId);
    Task<AppointmentDto> CancelAppointmentAsync(int appointmentId, string? reason = null);
    Task<AppointmentDto> CompleteAppointmentAsync(int appointmentId, string? notes = null);
    Task<AppointmentDto> RescheduleAppointmentAsync(int appointmentId, DateTime newDateTime);

    // Availability checking
    Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime dateTime);
    Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date);
    Task<bool> CheckAppointmentConflictAsync(
        int doctorId,
        DateTime dateTime,
        int? excludeAppointmentId = null
    );

    // Search and filtering
    Task<(IEnumerable<AppointmentDto> items, int totalCount)> SearchAppointmentsAsync(
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

    // Statistics and reporting
    Task<int> GetTotalAppointmentsCountAsync();
    Task<Dictionary<string, int>> GetAppointmentsByStatusStatsAsync();
    Task<Dictionary<string, int>> GetAppointmentsByDoctorStatsAsync();
    Task<IEnumerable<AppointmentDto>> GetOverdueAppointmentsAsync();
}
