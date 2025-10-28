using VisionCare.Application.DTOs.FollowUpDto;

namespace VisionCare.Application.Interfaces.FollowUp;

public interface IFollowUpService
{
    Task<IEnumerable<FollowUpDto>> GetAllFollowUpsAsync();
    Task<FollowUpDto?> GetFollowUpByIdAsync(int id);
    Task<FollowUpDto?> GetFollowUpByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<FollowUpDto>> GetFollowUpsByPatientIdAsync(int patientId);
    Task<IEnumerable<FollowUpDto>> GetFollowUpsByDoctorIdAsync(int doctorId);
    Task<(IEnumerable<FollowUpDto> items, int totalCount)> SearchFollowUpsAsync(FollowUpSearchRequest request);
    Task<FollowUpDto> CreateFollowUpAsync(CreateFollowUpRequest request);
    Task<FollowUpDto> UpdateFollowUpAsync(int id, UpdateFollowUpRequest request);
    Task<FollowUpDto> CompleteFollowUpAsync(int id);
    Task<FollowUpDto> CancelFollowUpAsync(int id, string? reason = null);
    Task<FollowUpDto> RescheduleFollowUpAsync(int id, DateTime newDate);
    Task<bool> DeleteFollowUpAsync(int id);
}
