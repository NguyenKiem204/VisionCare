using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.ServiceTypeDto;
using VisionCare.Application.Interfaces.ServiceTypes;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceTypesController : ControllerBase
{
    private readonly IServiceTypeService _serviceTypeService;

    public ServiceTypesController(IServiceTypeService serviceTypeService)
    {
        _serviceTypeService = serviceTypeService;
    }

    [HttpGet]
    [AllowAnonymous] // Customer needs to view service types for booking
    public async Task<IActionResult> GetAllServiceTypes()
    {
        var serviceTypes = await _serviceTypeService.GetAllServiceTypesAsync();
        return Ok(ApiResponse<IEnumerable<ServiceTypeDto>>.Ok(serviceTypes));
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Customer needs to view service types for booking
    public async Task<IActionResult> GetServiceTypeById(int id)
    {
        var serviceType = await _serviceTypeService.GetServiceTypeByIdAsync(id);
        if (serviceType == null)
        {
            return NotFound(
                ApiResponse<ServiceTypeDto>.Fail($"Service type with ID {id} not found.")
            );
        }
        return Ok(ApiResponse<ServiceTypeDto>.Ok(serviceType));
    }

    [HttpGet("search")]
    [AllowAnonymous] // Customer needs to search service types for booking
    public async Task<IActionResult> SearchServiceTypes(
        [FromQuery] string? keyword,
        [FromQuery] int? minDuration,
        [FromQuery] int? maxDuration,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool desc = false
    )
    {
        if (page < 1)
            page = 1;
        if (pageSize < 1)
            pageSize = 10;
        if (pageSize > 100)
            pageSize = 100;

        var result = await _serviceTypeService.SearchServiceTypesAsync(
            keyword,
            minDuration,
            maxDuration,
            page,
            pageSize,
            sortBy,
            desc
        );
        return Ok(
            PagedResponse<ServiceTypeDto>.Ok(result.items, result.totalCount, page, pageSize)
        );
    }

    [HttpPost]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> CreateServiceType([FromBody] CreateServiceTypeRequest request)
    {
        var serviceType = await _serviceTypeService.CreateServiceTypeAsync(request);
        return CreatedAtAction(
            nameof(GetServiceTypeById),
            new { id = serviceType.Id },
            ApiResponse<ServiceTypeDto>.Ok(serviceType)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> UpdateServiceType(
        int id,
        [FromBody] UpdateServiceTypeRequest request
    )
    {
        var serviceType = await _serviceTypeService.UpdateServiceTypeAsync(id, request);
        return Ok(ApiResponse<ServiceTypeDto>.Ok(serviceType));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> DeleteServiceType(int id)
    {
        var result = await _serviceTypeService.DeleteServiceTypeAsync(id);
        if (!result)
        {
            return NotFound(
                ApiResponse<ServiceTypeDto>.Fail($"Service type with ID {id} not found.")
            );
        }
        return Ok(ApiResponse<ServiceTypeDto>.Ok(null, "Service type deleted successfully"));
    }
}
