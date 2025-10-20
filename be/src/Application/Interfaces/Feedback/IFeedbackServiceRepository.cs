using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Feedback;

public interface IFeedbackServiceRepository
{
    Task<IEnumerable<FeedbackService>> GetAllAsync();
    Task<FeedbackService?> GetByIdAsync(int id);
    Task<FeedbackService?> GetByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<FeedbackService>> GetByServiceIdAsync(int serviceId);
    Task<IEnumerable<FeedbackService>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<FeedbackService>> GetByRatingRangeAsync(int minRating, int maxRating);
    Task<IEnumerable<FeedbackService>> GetUnrespondedAsync();
    Task<FeedbackService> AddAsync(FeedbackService feedback);
    Task UpdateAsync(FeedbackService feedback);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsForAppointmentAsync(int appointmentId);
    Task<double> GetAverageRatingByServiceIdAsync(int serviceId);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<FeedbackService>> GetPagedAsync(int page, int pageSize);
}
