using AutoMapper;
using VisionCare.Application.DTOs.CertificateDto;
using VisionCare.Application.Interfaces.Certificates;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Certificates;

public class CertificateService : ICertificateService
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly IMapper _mapper;

    public CertificateService(ICertificateRepository certificateRepository, IMapper mapper)
    {
        _certificateRepository = certificateRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CertificateDto>> GetAllCertificatesAsync()
    {
        var certificates = await _certificateRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CertificateDto>>(certificates);
    }

    public async Task<CertificateDto?> GetCertificateByIdAsync(int id)
    {
        var certificate = await _certificateRepository.GetByIdAsync(id);
        return certificate != null ? _mapper.Map<CertificateDto>(certificate) : null;
    }

    public async Task<CertificateDto> CreateCertificateAsync(CreateCertificateRequest request)
    {
        var certificate = _mapper.Map<Certificate>(request);
        certificate.Created = DateTime.UtcNow;
        certificate.LastModified = DateTime.UtcNow;

        var createdCertificate = await _certificateRepository.AddAsync(certificate);
        return _mapper.Map<CertificateDto>(createdCertificate);
    }

    public async Task<CertificateDto> UpdateCertificateAsync(int id, UpdateCertificateRequest request)
    {
        var existingCertificate = await _certificateRepository.GetByIdAsync(id);
        if (existingCertificate == null)
        {
            throw new ArgumentException($"Certificate with ID {id} not found.");
        }

        _mapper.Map(request, existingCertificate);
        existingCertificate.LastModified = DateTime.UtcNow;

        await _certificateRepository.UpdateAsync(existingCertificate);
        return _mapper.Map<CertificateDto>(existingCertificate);
    }

    public async Task<bool> DeleteCertificateAsync(int id)
    {
        var existingCertificate = await _certificateRepository.GetByIdAsync(id);
        if (existingCertificate == null)
        {
            return false;
        }

        await _certificateRepository.DeleteAsync(id);
        return true;
    }
}
