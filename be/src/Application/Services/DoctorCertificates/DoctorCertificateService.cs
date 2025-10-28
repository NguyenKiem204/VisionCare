using AutoMapper;
using VisionCare.Application.DTOs.DoctorCertificateDto;
using VisionCare.Application.Interfaces.DoctorCertificates;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.DoctorCertificates;

public class DoctorCertificateService : IDoctorCertificateService
{
    private readonly IDoctorCertificateRepository _doctorCertificateRepository;
    private readonly IMapper _mapper;

    public DoctorCertificateService(IDoctorCertificateRepository doctorCertificateRepository, IMapper mapper)
    {
        _doctorCertificateRepository = doctorCertificateRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorCertificateDto>> GetAllDoctorCertificatesAsync()
    {
        var doctorCertificates = await _doctorCertificateRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<DoctorCertificateDto>>(doctorCertificates);
    }

    public async Task<IEnumerable<DoctorCertificateDto>> GetCertificatesByDoctorAsync(int doctorId)
    {
        var doctorCertificates = await _doctorCertificateRepository.GetByDoctorIdAsync(doctorId);
        return _mapper.Map<IEnumerable<DoctorCertificateDto>>(doctorCertificates);
    }

    public async Task<DoctorCertificateDto?> GetDoctorCertificateByIdAsync(int id)
    {
        var doctorCertificate = await _doctorCertificateRepository.GetByIdAsync(id);
        return doctorCertificate != null ? _mapper.Map<DoctorCertificateDto>(doctorCertificate) : null;
    }

    public async Task<DoctorCertificateDto> CreateDoctorCertificateAsync(CreateDoctorCertificateRequest request)
    {
        var doctorCertificate = _mapper.Map<DoctorCertificate>(request);
        doctorCertificate.Created = DateTime.UtcNow;
        doctorCertificate.LastModified = DateTime.UtcNow;

        var createdDoctorCertificate = await _doctorCertificateRepository.AddAsync(doctorCertificate);
        return _mapper.Map<DoctorCertificateDto>(createdDoctorCertificate);
    }

    public async Task<DoctorCertificateDto> UpdateDoctorCertificateAsync(int id, UpdateDoctorCertificateRequest request)
    {
        var existingDoctorCertificate = await _doctorCertificateRepository.GetByIdAsync(id);
        if (existingDoctorCertificate == null)
        {
            throw new ArgumentException($"DoctorCertificate with ID {id} not found.");
        }

        _mapper.Map(request, existingDoctorCertificate);
        existingDoctorCertificate.LastModified = DateTime.UtcNow;

        await _doctorCertificateRepository.UpdateAsync(existingDoctorCertificate);
        return _mapper.Map<DoctorCertificateDto>(existingDoctorCertificate);
    }

    public async Task<bool> DeleteDoctorCertificateAsync(int id)
    {
        var existingDoctorCertificate = await _doctorCertificateRepository.GetByIdAsync(id);
        if (existingDoctorCertificate == null)
        {
            return false;
        }

        await _doctorCertificateRepository.DeleteAsync(id);
        return true;
    }
}
