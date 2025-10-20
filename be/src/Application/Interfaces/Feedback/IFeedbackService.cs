using VisionCare.Application.DTOs.FeedbackDto;

namespace VisionCare.Application.Interfaces.Feedback;

public interface IFeedbackService
{
    // Doctor Feedback Management
    Task<IEnumerable<FeedbackDoctorDto>> GetAllDoctorFeedbacksAsync();
    Task<FeedbackDoctorDto?> GetDoctorFeedbackByIdAsync(int id);
    Task<FeedbackDoctorDto?> GetDoctorFeedbackByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<FeedbackDoctorDto>> GetDoctorFeedbacksByDoctorIdAsync(int doctorId);
    Task<IEnumerable<FeedbackDoctorDto>> GetDoctorFeedbacksByPatientIdAsync(int patientId);
    Task<FeedbackDoctorDto> CreateDoctorFeedbackAsync(CreateFeedbackDoctorRequest request);
    Task<FeedbackDoctorDto> UpdateDoctorFeedbackAsync(int id, UpdateFeedbackDoctorRequest request);
    Task<FeedbackDoctorDto> RespondToDoctorFeedbackAsync(int id, RespondToFeedbackRequest request);
    Task<bool> DeleteDoctorFeedbackAsync(int id);

    // Service Feedback Management
    Task<IEnumerable<FeedbackServiceDto>> GetAllServiceFeedbacksAsync();
    Task<FeedbackServiceDto?> GetServiceFeedbackByIdAsync(int id);
    Task<FeedbackServiceDto?> GetServiceFeedbackByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<FeedbackServiceDto>> GetServiceFeedbacksByServiceIdAsync(int serviceId);
    Task<IEnumerable<FeedbackServiceDto>> GetServiceFeedbacksByPatientIdAsync(int patientId);
    Task<FeedbackServiceDto> CreateServiceFeedbackAsync(CreateFeedbackServiceRequest request);
    Task<FeedbackServiceDto> UpdateServiceFeedbackAsync(
        int id,
        UpdateFeedbackServiceRequest request
    );
    Task<FeedbackServiceDto> RespondToServiceFeedbackAsync(
        int id,
        RespondToServiceFeedbackRequest request
    );
    Task<bool> DeleteServiceFeedbackAsync(int id);

    // Search and Analytics
    Task<IEnumerable<FeedbackDoctorDto>> SearchDoctorFeedbacksAsync(FeedbackSearchRequest request);
    Task<IEnumerable<FeedbackServiceDto>> SearchServiceFeedbacksAsync(
        ServiceFeedbackSearchRequest request
    );
    Task<double> GetDoctorAverageRatingAsync(int doctorId);
    Task<double> GetServiceAverageRatingAsync(int serviceId);
    Task<IEnumerable<FeedbackDoctorDto>> GetUnrespondedDoctorFeedbacksAsync();
    Task<IEnumerable<FeedbackServiceDto>> GetUnrespondedServiceFeedbacksAsync();
    Task<int> GetTotalDoctorFeedbacksCountAsync();
    Task<int> GetTotalServiceFeedbacksCountAsync();
}
