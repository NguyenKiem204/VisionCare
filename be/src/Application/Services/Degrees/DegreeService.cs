using AutoMapper;
using VisionCare.Application.DTOs.DegreeDto;
using VisionCare.Application.Interfaces.Degrees;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Degrees;

public class DegreeService : IDegreeService
{
    private readonly IDegreeRepository _degreeRepository;
    private readonly IMapper _mapper;

    public DegreeService(IDegreeRepository degreeRepository, IMapper mapper)
    {
        _degreeRepository = degreeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DegreeDto>> GetAllDegreesAsync()
    {
        var degrees = await _degreeRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<DegreeDto>>(degrees);
    }

    public async Task<DegreeDto?> GetDegreeByIdAsync(int id)
    {
        var degree = await _degreeRepository.GetByIdAsync(id);
        return degree != null ? _mapper.Map<DegreeDto>(degree) : null;
    }

    public async Task<DegreeDto> CreateDegreeAsync(CreateDegreeRequest request)
    {
        var degree = _mapper.Map<Degree>(request);
        degree.Created = DateTime.UtcNow;
        degree.LastModified = DateTime.UtcNow;

        var createdDegree = await _degreeRepository.AddAsync(degree);
        return _mapper.Map<DegreeDto>(createdDegree);
    }

    public async Task<DegreeDto> UpdateDegreeAsync(int id, UpdateDegreeRequest request)
    {
        var existingDegree = await _degreeRepository.GetByIdAsync(id);
        if (existingDegree == null)
        {
            throw new ArgumentException($"Degree with ID {id} not found.");
        }

        _mapper.Map(request, existingDegree);
        existingDegree.LastModified = DateTime.UtcNow;

        await _degreeRepository.UpdateAsync(existingDegree);
        return _mapper.Map<DegreeDto>(existingDegree);
    }

    public async Task<bool> DeleteDegreeAsync(int id)
    {
        var existingDegree = await _degreeRepository.GetByIdAsync(id);
        if (existingDegree == null)
        {
            return false;
        }

        await _degreeRepository.DeleteAsync(id);
        return true;
    }
}
