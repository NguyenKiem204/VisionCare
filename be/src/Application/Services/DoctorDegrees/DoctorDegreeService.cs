using AutoMapper;
using VisionCare.Application.DTOs.DoctorDegreeDto;
using VisionCare.Application.Interfaces.DoctorDegrees;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.DoctorDegrees;

public class DoctorDegreeService : IDoctorDegreeService
{
    private readonly IDoctorDegreeRepository _doctorDegreeRepository;
    private readonly IMapper _mapper;

    public DoctorDegreeService(IDoctorDegreeRepository doctorDegreeRepository, IMapper mapper)
    {
        _doctorDegreeRepository = doctorDegreeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorDegreeDto>> GetAllDoctorDegreesAsync()
    {
        var doctorDegrees = await _doctorDegreeRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<DoctorDegreeDto>>(doctorDegrees);
    }

    public async Task<IEnumerable<DoctorDegreeDto>> GetDegreesByDoctorAsync(int doctorId)
    {
        var doctorDegrees = await _doctorDegreeRepository.GetByDoctorIdAsync(doctorId);
        return _mapper.Map<IEnumerable<DoctorDegreeDto>>(doctorDegrees);
    }

    public async Task<DoctorDegreeDto?> GetDoctorDegreeByIdAsync(int id)
    {
        var doctorDegree = await _doctorDegreeRepository.GetByIdAsync(id);
        return doctorDegree != null ? _mapper.Map<DoctorDegreeDto>(doctorDegree) : null;
    }

    public async Task<DoctorDegreeDto> CreateDoctorDegreeAsync(CreateDoctorDegreeRequest request)
    {
        var doctorDegree = _mapper.Map<DoctorDegree>(request);
        doctorDegree.Created = DateTime.UtcNow;
        doctorDegree.LastModified = DateTime.UtcNow;

        var createdDoctorDegree = await _doctorDegreeRepository.AddAsync(doctorDegree);
        return _mapper.Map<DoctorDegreeDto>(createdDoctorDegree);
    }

    public async Task<DoctorDegreeDto> UpdateDoctorDegreeAsync(int id, UpdateDoctorDegreeRequest request)
    {
        var existingDoctorDegree = await _doctorDegreeRepository.GetByIdAsync(id);
        if (existingDoctorDegree == null)
        {
            throw new ArgumentException($"DoctorDegree with ID {id} not found.");
        }

        _mapper.Map(request, existingDoctorDegree);
        existingDoctorDegree.LastModified = DateTime.UtcNow;

        await _doctorDegreeRepository.UpdateAsync(existingDoctorDegree);
        return _mapper.Map<DoctorDegreeDto>(existingDoctorDegree);
    }

    public async Task<bool> DeleteDoctorDegreeAsync(int id)
    {
        var existingDoctorDegree = await _doctorDegreeRepository.GetByIdAsync(id);
        if (existingDoctorDegree == null)
        {
            return false;
        }

        await _doctorDegreeRepository.DeleteAsync(id);
        return true;
    }
}
