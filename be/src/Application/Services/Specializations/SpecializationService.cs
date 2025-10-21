using AutoMapper;
using VisionCare.Application.DTOs.SpecializationDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Specializations;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Specializations;

public class SpecializationService : ISpecializationService
{
    private readonly ISpecializationRepository _specializationRepository;
    private readonly IMapper _mapper;

    public SpecializationService(ISpecializationRepository specializationRepository, IMapper mapper)
    {
        _specializationRepository = specializationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SpecializationDto>> GetAllSpecializationsAsync()
    {
        var specializations = await _specializationRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SpecializationDto>>(specializations);
    }

    public async Task<SpecializationDto?> GetSpecializationByIdAsync(int id)
    {
        var specialization = await _specializationRepository.GetByIdAsync(id);
        return specialization != null ? _mapper.Map<SpecializationDto>(specialization) : null;
    }

    public async Task<SpecializationDto> CreateSpecializationAsync(
        CreateSpecializationRequest request
    )
    {
        // Check if specialization name already exists
        var existingSpecialization = await _specializationRepository.GetByNameAsync(
            request.SpecializationName
        );
        if (existingSpecialization != null)
        {
            throw new ValidationException("Specialization with this name already exists.");
        }

        // Use AutoMapper to create entity from DTO
        var specialization = _mapper.Map<Specialization>(request);
        specialization.SpecializationStatus = "Active";
        specialization.Created = DateTime.UtcNow;

        var createdSpecialization = await _specializationRepository.AddAsync(specialization);
        return _mapper.Map<SpecializationDto>(createdSpecialization);
    }

    public async Task<SpecializationDto> UpdateSpecializationAsync(
        int id,
        UpdateSpecializationRequest request
    )
    {
        var existingSpecialization = await _specializationRepository.GetByIdAsync(id);
        if (existingSpecialization == null)
        {
            throw new NotFoundException($"Specialization with ID {id} not found.");
        }

        // Check if new name conflicts with existing specializations
        if (request.SpecializationName != existingSpecialization.SpecializationName)
        {
            var nameConflict = await _specializationRepository.GetByNameAsync(
                request.SpecializationName
            );
            if (nameConflict != null && nameConflict.Id != id)
            {
                throw new ValidationException("Specialization with this name already exists.");
            }
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingSpecialization);

        await _specializationRepository.UpdateAsync(existingSpecialization);
        return _mapper.Map<SpecializationDto>(existingSpecialization);
    }

    public async Task<bool> DeleteSpecializationAsync(int id)
    {
        var existingSpecialization = await _specializationRepository.GetByIdAsync(id);
        if (existingSpecialization == null)
        {
            return false;
        }

        // Check if specialization is being used by doctors
        var doctorsUsingSpecialization =
            await _specializationRepository.GetDoctorsCountBySpecializationAsync(id);
        if (doctorsUsingSpecialization > 0)
        {
            throw new ValidationException(
                "Cannot delete specialization that is being used by doctors."
            );
        }

        await _specializationRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<SpecializationDto>> GetActiveSpecializationsAsync()
    {
        var specializations = await _specializationRepository.GetActiveAsync();
        return _mapper.Map<IEnumerable<SpecializationDto>>(specializations);
    }

    public async Task<SpecializationDto> ActivateSpecializationAsync(int specializationId)
    {
        var specialization = await _specializationRepository.GetByIdAsync(specializationId);
        if (specialization == null)
        {
            throw new NotFoundException($"Specialization with ID {specializationId} not found.");
        }

        specialization.Activate();

        await _specializationRepository.UpdateAsync(specialization);
        return _mapper.Map<SpecializationDto>(specialization);
    }

    public async Task<SpecializationDto> DeactivateSpecializationAsync(int specializationId)
    {
        var specialization = await _specializationRepository.GetByIdAsync(specializationId);
        if (specialization == null)
        {
            throw new NotFoundException($"Specialization with ID {specializationId} not found.");
        }

        specialization.Deactivate();

        await _specializationRepository.UpdateAsync(specialization);
        return _mapper.Map<SpecializationDto>(specialization);
    }

    public async Task<IEnumerable<SpecializationDto>> SearchSpecializationsAsync(string keyword)
    {
        var specializations = await _specializationRepository.SearchAsync(keyword);
        return _mapper.Map<IEnumerable<SpecializationDto>>(specializations);
    }

    public async Task<int> GetTotalSpecializationsCountAsync()
    {
        return await _specializationRepository.GetTotalCountAsync();
    }

    public async Task<int> GetActiveSpecializationsCountAsync()
    {
        return await _specializationRepository.GetActiveCountAsync();
    }

    public async Task<Dictionary<string, int>> GetSpecializationUsageStatsAsync()
    {
        return await _specializationRepository.GetUsageStatsAsync();
    }
}
