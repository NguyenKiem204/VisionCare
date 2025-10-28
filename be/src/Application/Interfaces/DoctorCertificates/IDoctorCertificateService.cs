using VisionCare.Application.DTOs.DoctorCertificateDto;

namespace VisionCare.Application.Interfaces.DoctorCertificates;

public interface IDoctorCertificateService
{
    Task<IEnumerable<DoctorCertificateDto>> GetAllDoctorCertificatesAsync();
    Task<IEnumerable<DoctorCertificateDto>> GetCertificatesByDoctorAsync(int doctorId);
    Task<DoctorCertificateDto?> GetDoctorCertificateByIdAsync(int id);
    Task<DoctorCertificateDto> CreateDoctorCertificateAsync(CreateDoctorCertificateRequest request);
    Task<DoctorCertificateDto> UpdateDoctorCertificateAsync(int id, UpdateDoctorCertificateRequest request);
    Task<bool> DeleteDoctorCertificateAsync(int id);
}
