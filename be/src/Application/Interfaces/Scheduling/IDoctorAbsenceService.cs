using VisionCare.Application.DTOs;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface IDoctorAbsenceService
{
    Task<IEnumerable<DoctorAbsenceDto>> GetAllAsync();
    Task<DoctorAbsenceDto?> GetByIdAsync(int id);
    Task<IEnumerable<DoctorAbsenceDto>> GetByDoctorIdAsync(int doctorId);
    Task<IEnumerable<DoctorAbsenceDto>> GetPendingAsync();
    Task<DoctorAbsenceDto> CreateAsync(CreateDoctorAbsenceRequest request);
    Task<DoctorAbsenceDto> UpdateAsync(int id, UpdateDoctorAbsenceRequest request);
    Task<DoctorAbsenceDto> ApproveAsync(int id);
    Task<DoctorAbsenceDto> RejectAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<Dictionary<string, int>> HandleAbsenceAppointmentsAsync(
        int absenceId,
        HandleAbsenceAppointmentsRequest request
    );
}

