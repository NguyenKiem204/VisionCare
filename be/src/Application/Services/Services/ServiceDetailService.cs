using AutoMapper;
using VisionCare.Application.DTOs.ServiceDetailDto;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Services;

public class ServiceDetailService : IServiceDetailService
{
    private readonly IServiceDetailRepository _serviceDetailRepository;
    private readonly IMapper _mapper;

    public ServiceDetailService(IServiceDetailRepository serviceDetailRepository, IMapper mapper)
    {
        _serviceDetailRepository = serviceDetailRepository;
        _mapper = mapper;
    }

    public async Task<ServiceDetailDto?> GetServiceDetailByIdAsync(int id)
    {
        var serviceDetail = await _serviceDetailRepository.GetByIdAsync(id);
        return serviceDetail != null ? _mapper.Map<ServiceDetailDto>(serviceDetail) : null;
    }

    public async Task<ServiceDetailDto?> GetByServiceIdAndTypeIdAsync(int serviceId, int serviceTypeId)
    {
        var serviceDetail = await _serviceDetailRepository.GetByServiceIdAndTypeIdAsync(serviceId, serviceTypeId);
        return serviceDetail != null ? _mapper.Map<ServiceDetailDto>(serviceDetail) : null;
    }

    public async Task<IEnumerable<ServiceDetailDto>> GetByServiceTypeIdAsync(int serviceTypeId)
    {
        var serviceDetails = await _serviceDetailRepository.GetByServiceTypeIdAsync(serviceTypeId);
        return _mapper.Map<IEnumerable<ServiceDetailDto>>(serviceDetails);
    }
}

