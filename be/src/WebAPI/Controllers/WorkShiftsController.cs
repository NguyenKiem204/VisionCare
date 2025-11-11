using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.WorkShiftDto;
using VisionCare.Application.Interfaces.WorkShifts;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAdmin")]
public class WorkShiftsController : ControllerBase
{
    private readonly IWorkShiftService _workShiftService;

    public WorkShiftsController(IWorkShiftService workShiftService)
    {
        _workShiftService = workShiftService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWorkShifts()
    {
        var shifts = await _workShiftService.GetAllWorkShiftsAsync();
        return Ok(ApiResponse<IEnumerable<WorkShiftDto>>.Ok(shifts));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveWorkShifts()
    {
        var shifts = await _workShiftService.GetActiveWorkShiftsAsync();
        return Ok(ApiResponse<IEnumerable<WorkShiftDto>>.Ok(shifts));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkShiftById(int id)
    {
        var shift = await _workShiftService.GetWorkShiftByIdAsync(id);
        if (shift == null)
        {
            return NotFound(ApiResponse<WorkShiftDto>.Fail($"WorkShift with ID {id} not found."));
        }
        return Ok(ApiResponse<WorkShiftDto>.Ok(shift));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchWorkShifts(
        [FromQuery] string? keyword,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var request = new WorkShiftSearchRequest
        {
            Keyword = keyword,
            IsActive = isActive,
            Page = page,
            PageSize = pageSize
        };
        var shifts = await _workShiftService.SearchWorkShiftsAsync(request);
        return Ok(ApiResponse<IEnumerable<WorkShiftDto>>.Ok(shifts));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateWorkShift([FromBody] CreateWorkShiftRequest request)
    {
        var shift = await _workShiftService.CreateWorkShiftAsync(request);
        return CreatedAtAction(
            nameof(GetWorkShiftById),
            new { id = shift.Id },
            ApiResponse<WorkShiftDto>.Ok(shift)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateWorkShift(int id, [FromBody] UpdateWorkShiftRequest request)
    {
        var shift = await _workShiftService.UpdateWorkShiftAsync(id, request);
        return Ok(ApiResponse<WorkShiftDto>.Ok(shift));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteWorkShift(int id)
    {
        var result = await _workShiftService.DeleteWorkShiftAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<WorkShiftDto>.Fail($"WorkShift with ID {id} not found."));
        }
        return Ok(ApiResponse<WorkShiftDto>.Ok(null, "WorkShift deleted successfully"));
    }
}

