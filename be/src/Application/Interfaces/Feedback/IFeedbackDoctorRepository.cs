using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Feedback;

public interface IFeedbackDoctorRepository
{
    Task<IEnumerable<FeedbackDoctor>> GetAllAsync();
    Task<FeedbackDoctor?> GetByIdAsync(int id);
    Task<FeedbackDoctor?> GetByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<FeedbackDoctor>> GetByDoctorIdAsync(int doctorId);
    Task<IEnumerable<FeedbackDoctor>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<FeedbackDoctor>> GetByRatingRangeAsync(int minRating, int maxRating);
    Task<IEnumerable<FeedbackDoctor>> GetUnrespondedAsync();
    Task<FeedbackDoctor> AddAsync(FeedbackDoctor feedback);
    Task UpdateAsync(FeedbackDoctor feedback);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsForAppointmentAsync(int appointmentId);
    Task<double> GetAverageRatingByDoctorIdAsync(int doctorId);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<FeedbackDoctor>> GetPagedAsync(int page, int pageSize);
}
