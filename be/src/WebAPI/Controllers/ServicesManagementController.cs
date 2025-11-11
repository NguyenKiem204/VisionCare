using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.ServiceDto;
using VisionCare.Application.Interfaces.Services;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesManagementController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServicesManagementController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    [AllowAnonymous] // Customer needs to view services for booking
    public async Task<IActionResult> GetAllServices()
    {
        var services = await _serviceService.GetAllServicesAsync();
        return Ok(ApiResponse<IEnumerable<ServiceDto>>.Ok(services));
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Customer needs to view services for booking
    public async Task<IActionResult> GetServiceById(int id)
    {
        var service = await _serviceService.GetServiceByIdAsync(id);
        if (service == null)
        {
            return NotFound(ApiResponse<ServiceDto>.Fail($"Service with ID {id} not found."));
        }
        return Ok(ApiResponse<ServiceDto>.Ok(service));
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetServiceByName(string name)
    {
        var service = await _serviceService.GetServiceByNameAsync(name);
        if (service == null)
        {
            return NotFound(ApiResponse<ServiceDto>.Fail($"Service with name '{name}' not found."));
        }
        return Ok(ApiResponse<ServiceDto>.Ok(service));
    }

    [HttpGet("by-specialization/{specializationId}")]
    public async Task<IActionResult> GetServicesBySpecialization(int specializationId)
    {
        var services = await _serviceService.GetServicesBySpecializationAsync(specializationId);
        return Ok(ApiResponse<IEnumerable<ServiceDto>>.Ok(services));
    }

    [HttpGet("active")]
    [AllowAnonymous] // Customer needs to view active services for booking
    public async Task<IActionResult> GetActiveServices()
    {
        var services = await _serviceService.GetActiveServicesAsync();
        return Ok(ApiResponse<IEnumerable<ServiceDto>>.Ok(services));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest request)
    {
        var service = await _serviceService.CreateServiceAsync(request);
        return CreatedAtAction(
            nameof(GetServiceById),
            new { id = service.Id },
            ApiResponse<ServiceDto>.Ok(service)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateService(int id, [FromBody] UpdateServiceRequest request)
    {
        var service = await _serviceService.UpdateServiceAsync(id, request);
        return Ok(ApiResponse<ServiceDto>.Ok(service));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteService(int id)
    {
        var result = await _serviceService.DeleteServiceAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<ServiceDto>.Fail($"Service with ID {id} not found."));
        }
        return Ok(ApiResponse<ServiceDto>.Ok(null, "Service deleted successfully"));
    }

    [HttpPut("{id}/activate")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ActivateService(int id)
    {
        var service = await _serviceService.ActivateServiceAsync(id);
        return Ok(ApiResponse<ServiceDto>.Ok(service));
    }

    [HttpPut("{id}/deactivate")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeactivateService(int id)
    {
        var service = await _serviceService.DeactivateServiceAsync(id);
        return Ok(ApiResponse<ServiceDto>.Ok(service));
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchServices([FromBody] ServiceSearchRequest request)
    {
        var services = await _serviceService.SearchServicesAsync(request);
        return Ok(ApiResponse<IEnumerable<ServiceDto>>.Ok(services));
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetTotalServicesCount()
    {
        var count = await _serviceService.GetTotalServicesCountAsync();
        return Ok(ApiResponse<int>.Ok(count));
    }
}
