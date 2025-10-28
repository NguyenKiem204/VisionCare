using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.FollowUp;

public interface IFollowUpRepository
{
    Task<IEnumerable<Domain.Entities.FollowUp>> GetAllAsync();
    Task<Domain.Entities.FollowUp?> GetByIdAsync(int id);
    Task<Domain.Entities.FollowUp?> GetByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<Domain.Entities.FollowUp>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<Domain.Entities.FollowUp>> GetByDoctorIdAsync(int doctorId);
    Task<Domain.Entities.FollowUp> AddAsync(Domain.Entities.FollowUp followUp);
    Task UpdateAsync(Domain.Entities.FollowUp followUp);
    Task DeleteAsync(int id);
    Task<(IEnumerable<Domain.Entities.FollowUp> items, int totalCount)> SearchAsync(
        int? patientId,
        int? doctorId,
        string? status,
        DateTime? fromDate,
        DateTime? toDate,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    );
}
