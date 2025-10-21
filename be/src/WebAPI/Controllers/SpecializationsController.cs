using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.SpecializationDto;
using VisionCare.Application.Interfaces.Specializations;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecializationsController : ControllerBase
{
    private readonly ISpecializationService _specializationService;

    public SpecializationsController(ISpecializationService specializationService)
    {
        _specializationService = specializationService;
    }

    /// <summary>
    /// Get all specializations
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetSpecializations()
    {
        var specializations = await _specializationService.GetAllSpecializationsAsync();
        return Ok(specializations);
    }

    /// <summary>
    /// Get specialization by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SpecializationDto>> GetSpecialization(int id)
    {
        var specialization = await _specializationService.GetSpecializationByIdAsync(id);
        if (specialization == null)
        {
            return NotFound();
        }
        return Ok(specialization);
    }

    /// <summary>
    /// Create a new specialization
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SpecializationDto>> CreateSpecialization(
        CreateSpecializationRequest request
    )
    {
        var specialization = await _specializationService.CreateSpecializationAsync(request);
        return CreatedAtAction(
            nameof(GetSpecialization),
            new { id = specialization.Id },
            specialization
        );
    }

    /// <summary>
    /// Update specialization
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<SpecializationDto>> UpdateSpecialization(
        int id,
        UpdateSpecializationRequest request
    )
    {
        var specialization = await _specializationService.UpdateSpecializationAsync(id, request);
        return Ok(specialization);
    }

    /// <summary>
    /// Delete specialization
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSpecialization(int id)
    {
        var result = await _specializationService.DeleteSpecializationAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// Get active specializations
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<SpecializationDto>>> GetActiveSpecializations()
    {
        var specializations = await _specializationService.GetActiveSpecializationsAsync();
        return Ok(specializations);
    }

    /// <summary>
    /// Activate specialization
    /// </summary>
    [HttpPut("{id}/activate")]
    public async Task<ActionResult<SpecializationDto>> ActivateSpecialization(int id)
    {
        var specialization = await _specializationService.ActivateSpecializationAsync(id);
        return Ok(specialization);
    }

    /// <summary>
    /// Deactivate specialization
    /// </summary>
    [HttpPut("{id}/deactivate")]
    public async Task<ActionResult<SpecializationDto>> DeactivateSpecialization(int id)
    {
        var specialization = await _specializationService.DeactivateSpecializationAsync(id);
        return Ok(specialization);
    }

    /// <summary>
    /// Search specializations
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<SpecializationDto>>> SearchSpecializations(
        [FromQuery] string keyword
    )
    {
        var specializations = await _specializationService.SearchSpecializationsAsync(keyword);
        return Ok(specializations);
    }

    /// <summary>
    /// Get specialization statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<ActionResult<object>> GetSpecializationStatistics()
    {
        var totalCount = await _specializationService.GetTotalSpecializationsCountAsync();
        var activeCount = await _specializationService.GetActiveSpecializationsCountAsync();
        var usageStats = await _specializationService.GetSpecializationUsageStatsAsync();

        return Ok(
            new
            {
                TotalCount = totalCount,
                ActiveCount = activeCount,
                UsageStats = usageStats,
            }
        );
    }
}
