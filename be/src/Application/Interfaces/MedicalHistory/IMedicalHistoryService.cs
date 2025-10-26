using VisionCare.Application.DTOs.MedicalHistoryDto;

namespace VisionCare.Application.Interfaces.MedicalHistory;

public interface IMedicalHistoryService
{
    Task<IEnumerable<MedicalHistoryDto>> GetAllMedicalHistoriesAsync();
    Task<MedicalHistoryDto?> GetMedicalHistoryByIdAsync(int id);
    Task<MedicalHistoryDto?> GetMedicalHistoryByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<MedicalHistoryDto>> GetMedicalHistoriesByPatientIdAsync(int patientId);
    Task<IEnumerable<MedicalHistoryDto>> GetMedicalHistoriesByDoctorIdAsync(int doctorId);
    Task<MedicalHistoryDto> CreateMedicalHistoryAsync(CreateMedicalHistoryRequest request);
    Task<MedicalHistoryDto> UpdateMedicalHistoryAsync(int id, UpdateMedicalHistoryRequest request);
    Task<bool> DeleteMedicalHistoryAsync(int id);
    Task<(IEnumerable<MedicalHistoryDto> items, int totalCount)> SearchMedicalHistoriesAsync(
        MedicalHistorySearchRequest request
    );
    Task<int> GetTotalMedicalHistoriesCountAsync();
    Task<bool> MedicalHistoryExistsForAppointmentAsync(int appointmentId);
}
