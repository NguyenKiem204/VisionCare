using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.MedicalHistory;

public interface IMedicalHistoryRepository
{
    Task<IEnumerable<Domain.Entities.MedicalHistory>> GetAllAsync();
    Task<Domain.Entities.MedicalHistory?> GetByIdAsync(int id);
    Task<Domain.Entities.MedicalHistory?> GetByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<Domain.Entities.MedicalHistory>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<Domain.Entities.MedicalHistory>> GetByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Domain.Entities.MedicalHistory>> SearchAsync(
        int? patientId,
        int? doctorId,
        DateTime? fromDate,
        DateTime? toDate,
        string? diagnosis
    );
    Task<Domain.Entities.MedicalHistory> AddAsync(Domain.Entities.MedicalHistory medicalHistory);
    Task UpdateAsync(Domain.Entities.MedicalHistory medicalHistory);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsForAppointmentAsync(int appointmentId);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Domain.Entities.MedicalHistory>> GetPagedAsync(int page, int pageSize);
}
