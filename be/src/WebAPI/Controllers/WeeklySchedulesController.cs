using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAdmin")]
public class WeeklySchedulesController : ControllerBase
{
    private readonly IWeeklyScheduleService _weeklyScheduleService;

    public WeeklySchedulesController(IWeeklyScheduleService weeklyScheduleService)
    {
        _weeklyScheduleService = weeklyScheduleService;
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<ActionResult<IEnumerable<WeeklyScheduleDto>>> GetByDoctor(int doctorId)
    {
        var schedules = await _weeklyScheduleService.GetByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<WeeklyScheduleDto>>.Ok(schedules));
    }

    [HttpGet("doctor/{doctorId}/active")]
    public async Task<ActionResult<IEnumerable<WeeklyScheduleDto>>> GetActiveByDoctor(int doctorId)
    {
        var schedules = await _weeklyScheduleService.GetActiveByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<WeeklyScheduleDto>>.Ok(schedules));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WeeklyScheduleDto>> GetById(int id)
    {
        var schedule = await _weeklyScheduleService.GetByIdAsync(id);
        if (schedule == null)
        {
            return NotFound(
                ApiResponse<WeeklyScheduleDto>.Fail($"Weekly schedule with ID {id} not found.")
            );
        }
        return Ok(ApiResponse<WeeklyScheduleDto>.Ok(schedule));
    }

    [HttpPost]
    public async Task<ActionResult<WeeklyScheduleDto>> Create(
        [FromBody] CreateWeeklyScheduleRequest request
    )
    {
        try
        {
            var schedule = await _weeklyScheduleService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = schedule.Id },
                ApiResponse<WeeklyScheduleDto>.Ok(schedule)
            );
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<WeeklyScheduleDto>.Fail(ex.Message));
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<WeeklyScheduleDto>.Fail(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<WeeklyScheduleDto>> Update(
        int id,
        [FromBody] UpdateWeeklyScheduleRequest request
    )
    {
        try
        {
            var schedule = await _weeklyScheduleService.UpdateAsync(id, request);
            return Ok(ApiResponse<WeeklyScheduleDto>.Ok(schedule));
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<WeeklyScheduleDto>.Fail(ex.Message));
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<WeeklyScheduleDto>.Fail(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _weeklyScheduleService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.Fail($"Weekly schedule with ID {id} not found."));
        }
        return NoContent();
    }
}
