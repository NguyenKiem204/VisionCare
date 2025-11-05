using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.WebAPI.Responses;
 

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAdmin")]
public class ScheduleGenerationController : ControllerBase
{
    private readonly IScheduleGenerationService _generationService;

    public ScheduleGenerationController(IScheduleGenerationService generationService)
    {
        _generationService = generationService;
    }

    [HttpPost("doctor/{doctorId}")]
    public async Task<ActionResult<int>> GenerateForDoctor(
        int doctorId,
        [FromQuery] int daysAhead = 14
    )
    {
        // Use DoctorSchedules (flexible recurrent) to generate
        var count = await _generationService.GenerateSchedulesFromAllDoctorSchedulesAsync(doctorId, daysAhead);
        return Ok(ApiResponse<int>.Ok(count, $"Generated {count} schedules for doctor {doctorId} from DoctorSchedules."));
    }

    [HttpPost("all")]
    public async Task<ActionResult<int>> GenerateForAll([FromQuery] int daysAhead = 14)
    {
        // Service handles iterating all doctors
        var total = await _generationService.GenerateSchedulesForAllDoctorsAsync(daysAhead);
        return Ok(ApiResponse<int>.Ok(total, $"Generated {total} schedules for all doctors from DoctorSchedules."));
    }

    [HttpPost("doctor/{doctorId}/range")]
    public async Task<ActionResult<int>> GenerateForDateRange(
        int doctorId,
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate
    )
    {
        if (endDate < startDate)
        {
            return BadRequest(ApiResponse<int>.Fail("End date must be after start date."));
        }

        var count = await _generationService.GenerateSchedulesForDateRangeAsync(
            doctorId,
            startDate,
            endDate
        );
        return Ok(
            ApiResponse<int>.Ok(
                count,
                $"Generated {count} schedules for doctor {doctorId} from {startDate} to {endDate}."
            )
        );
    }
}
