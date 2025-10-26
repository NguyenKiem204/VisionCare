using AutoMapper;
using VisionCare.Application.DTOs.ServiceTypeDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.ServiceTypes;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.ServiceTypes;

public class ServiceTypeService : IServiceTypeService
{
    private readonly IServiceTypeRepository _serviceTypeRepository;
    private readonly IMapper _mapper;

    public ServiceTypeService(IServiceTypeRepository serviceTypeRepository, IMapper mapper)
    {
        _serviceTypeRepository = serviceTypeRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceTypeDto>> GetAllServiceTypesAsync()
    {
        var serviceTypes = await _serviceTypeRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ServiceTypeDto>>(serviceTypes);
    }

    public async Task<ServiceTypeDto?> GetServiceTypeByIdAsync(int id)
    {
        var serviceType = await _serviceTypeRepository.GetByIdAsync(id);
        return serviceType != null ? _mapper.Map<ServiceTypeDto>(serviceType) : null;
    }

    public async Task<ServiceTypeDto> CreateServiceTypeAsync(CreateServiceTypeRequest request)
    {
        var serviceType = _mapper.Map<ServiceType>(request);
        var createdServiceType = await _serviceTypeRepository.AddAsync(serviceType);
        return _mapper.Map<ServiceTypeDto>(createdServiceType);
    }

    public async Task<ServiceTypeDto> UpdateServiceTypeAsync(int id, UpdateServiceTypeRequest request)
    {
        var existingServiceType = await _serviceTypeRepository.GetByIdAsync(id);
        if (existingServiceType == null)
        {
            throw new NotFoundException($"Service type with ID {id} not found.");
        }

        _mapper.Map(request, existingServiceType);
        await _serviceTypeRepository.UpdateAsync(existingServiceType);
        return _mapper.Map<ServiceTypeDto>(existingServiceType);
    }

    public async Task<bool> DeleteServiceTypeAsync(int id)
    {
        var existingServiceType = await _serviceTypeRepository.GetByIdAsync(id);
        if (existingServiceType == null)
        {
            return false;
        }

        await _serviceTypeRepository.DeleteAsync(id);
        return true;
    }

    public async Task<(IEnumerable<ServiceTypeDto> items, int totalCount)> SearchServiceTypesAsync(
        string? keyword,
        int? minDuration,
        int? maxDuration,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var result = await _serviceTypeRepository.SearchAsync(
            keyword,
            minDuration,
            maxDuration,
            page,
            pageSize,
            sortBy,
            desc
        );
        return (_mapper.Map<IEnumerable<ServiceTypeDto>>(result.items), result.totalCount);
    }
}
