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

    /// <summary>
    /// Get all services
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllServices()
    {
        var services = await _serviceService.GetAllServicesAsync();
        return Ok(ApiResponse<IEnumerable<ServiceDto>>.Ok(services));
    }

    /// <summary>
    /// Get service by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetServiceById(int id)
    {
        var service = await _serviceService.GetServiceByIdAsync(id);
        if (service == null)
        {
            return NotFound(ApiResponse<ServiceDto>.Fail($"Service with ID {id} not found."));
        }
        return Ok(ApiResponse<ServiceDto>.Ok(service));
    }

    /// <summary>
    /// Get service by name
    /// </summary>
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

    /// <summary>
    /// Get services by specialization
    /// </summary>
    [HttpGet("by-specialization/{specializationId}")]
    public async Task<IActionResult> GetServicesBySpecialization(int specializationId)
    {
        var services = await _serviceService.GetServicesBySpecializationAsync(specializationId);
        return Ok(ApiResponse<IEnumerable<ServiceDto>>.Ok(services));
    }

    /// <summary>
    /// Get active services only
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveServices()
    {
        var services = await _serviceService.GetActiveServicesAsync();
        return Ok(ApiResponse<IEnumerable<ServiceDto>>.Ok(services));
    }

    /// <summary>
    /// Create a new service
    /// </summary>
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

    /// <summary>
    /// Update service
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateService(int id, [FromBody] UpdateServiceRequest request)
    {
        var service = await _serviceService.UpdateServiceAsync(id, request);
        return Ok(ApiResponse<ServiceDto>.Ok(service));
    }

    /// <summary>
    /// Delete service
    /// </summary>
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

    /// <summary>
    /// Activate service
    /// </summary>
    [HttpPut("{id}/activate")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ActivateService(int id)
    {
        var service = await _serviceService.ActivateServiceAsync(id);
        return Ok(ApiResponse<ServiceDto>.Ok(service));
    }

    /// <summary>
    /// Deactivate service
    /// </summary>
    [HttpPut("{id}/deactivate")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeactivateService(int id)
    {
        var service = await _serviceService.DeactivateServiceAsync(id);
        return Ok(ApiResponse<ServiceDto>.Ok(service));
    }

    /// <summary>
    /// Search services
    /// </summary>
    [HttpPost("search")]
    public async Task<IActionResult> SearchServices([FromBody] ServiceSearchRequest request)
    {
        var services = await _serviceService.SearchServicesAsync(request);
        return Ok(ApiResponse<IEnumerable<ServiceDto>>.Ok(services));
    }

    /// <summary>
    /// Get total services count
    /// </summary>
    [HttpGet("count")]
    public async Task<IActionResult> GetTotalServicesCount()
    {
        var count = await _serviceService.GetTotalServicesCountAsync();
        return Ok(ApiResponse<int>.Ok(count));
    }
}
