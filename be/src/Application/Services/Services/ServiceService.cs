using AutoMapper;
using VisionCare.Application.DTOs.ServiceDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IMapper _mapper;

    public ServiceService(IServiceRepository serviceRepository, IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
    {
        var services = await _serviceRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    public async Task<ServiceDto?> GetServiceByIdAsync(int id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        return service != null ? _mapper.Map<ServiceDto>(service) : null;
    }

    public async Task<ServiceDto?> GetServiceByNameAsync(string name)
    {
        var service = await _serviceRepository.GetByNameAsync(name);
        return service != null ? _mapper.Map<ServiceDto>(service) : null;
    }

    public async Task<IEnumerable<ServiceDto>> GetServicesBySpecializationAsync(
        int specializationId
    )
    {
        var services = await _serviceRepository.GetBySpecializationAsync(specializationId);
        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    public async Task<IEnumerable<ServiceDto>> GetActiveServicesAsync()
    {
        var services = await _serviceRepository.GetActiveAsync();
        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    public async Task<ServiceDto> CreateServiceAsync(CreateServiceRequest request)
    {
        // Check if service name already exists
        if (await _serviceRepository.NameExistsAsync(request.Name))
        {
            throw new ValidationException("Service with this name already exists.");
        }

        // Use AutoMapper to create entity from DTO
        var service = _mapper.Map<Service>(request);
        service.Status = "Active";
        service.Created = DateTime.UtcNow;

        var createdService = await _serviceRepository.AddAsync(service);
        return _mapper.Map<ServiceDto>(createdService);
    }

    public async Task<ServiceDto> UpdateServiceAsync(int id, UpdateServiceRequest request)
    {
        var existingService = await _serviceRepository.GetByIdAsync(id);
        if (existingService == null)
        {
            throw new NotFoundException($"Service with ID {id} not found.");
        }

        // Check if new name conflicts with existing services
        if (
            request.Name != existingService.Name
            && await _serviceRepository.NameExistsAsync(request.Name, id)
        )
        {
            throw new ValidationException("Service with this name already exists.");
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingService);

        await _serviceRepository.UpdateAsync(existingService);
        return _mapper.Map<ServiceDto>(existingService);
    }

    public async Task<bool> DeleteServiceAsync(int id)
    {
        var existingService = await _serviceRepository.GetByIdAsync(id);
        if (existingService == null)
        {
            return false;
        }

        await _serviceRepository.DeleteAsync(id);
        return true;
    }

    public async Task<ServiceDto> ActivateServiceAsync(int id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service == null)
        {
            throw new NotFoundException($"Service with ID {id} not found.");
        }

        // Use domain method to activate service
        service.Activate();

        await _serviceRepository.UpdateAsync(service);
        return _mapper.Map<ServiceDto>(service);
    }

    public async Task<ServiceDto> DeactivateServiceAsync(int id)
    {
        var service = await _serviceRepository.GetByIdAsync(id);
        if (service == null)
        {
            throw new NotFoundException($"Service with ID {id} not found.");
        }

        // Use domain method to deactivate service
        service.Deactivate();

        await _serviceRepository.UpdateAsync(service);
        return _mapper.Map<ServiceDto>(service);
    }

    public async Task<IEnumerable<ServiceDto>> SearchServicesAsync(ServiceSearchRequest request)
    {
        var services = await _serviceRepository.SearchAsync(
            request.Keyword ?? string.Empty,
            request.SpecializationId,
            request.Status
        );

        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }

    public async Task<int> GetTotalServicesCountAsync()
    {
        return await _serviceRepository.GetTotalCountAsync();
    }

    public async Task<bool> ServiceNameExistsAsync(string name, int? excludeId = null)
    {
        return await _serviceRepository.NameExistsAsync(name, excludeId);
    }
}
