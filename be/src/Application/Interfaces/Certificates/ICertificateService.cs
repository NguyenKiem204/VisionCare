using VisionCare.Application.DTOs.CertificateDto;

namespace VisionCare.Application.Interfaces.Certificates;

public interface ICertificateService
{
    Task<IEnumerable<CertificateDto>> GetAllCertificatesAsync();
    Task<CertificateDto?> GetCertificateByIdAsync(int id);
    Task<CertificateDto> CreateCertificateAsync(CreateCertificateRequest request);
    Task<CertificateDto> UpdateCertificateAsync(int id, UpdateCertificateRequest request);
    Task<bool> DeleteCertificateAsync(int id);
}
