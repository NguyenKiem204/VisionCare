using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.ServiceDetailDto;
using VisionCare.Application.Interfaces.Services;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceDetailsController : ControllerBase
{
    private readonly IServiceDetailService _serviceDetailService;

    public ServiceDetailsController(IServiceDetailService serviceDetailService)
    {
        _serviceDetailService = serviceDetailService;
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Customer needs to view service details for booking
    public async Task<IActionResult> GetServiceDetailById(int id)
    {
        var serviceDetail = await _serviceDetailService.GetServiceDetailByIdAsync(id);
        if (serviceDetail == null)
        {
            return NotFound(ApiResponse<ServiceDetailDto>.Fail($"Service detail with ID {id} not found."));
        }
        return Ok(ApiResponse<ServiceDetailDto>.Ok(serviceDetail));
    }

    [HttpGet("by-service-type/{serviceTypeId}")]
    [AllowAnonymous] // Customer needs to view service details for booking
    public async Task<IActionResult> GetServiceDetailsByServiceTypeId(int serviceTypeId)
    {
        var serviceDetails = await _serviceDetailService.GetByServiceTypeIdAsync(serviceTypeId);
        return Ok(ApiResponse<IEnumerable<ServiceDetailDto>>.Ok(serviceDetails));
    }
}

