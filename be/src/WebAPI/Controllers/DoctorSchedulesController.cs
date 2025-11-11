using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.DoctorScheduleDto;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAdmin")]
public class DoctorSchedulesController : ControllerBase
{
    private readonly IDoctorScheduleService _doctorScheduleService;

    public DoctorSchedulesController(IDoctorScheduleService doctorScheduleService)
    {
        _doctorScheduleService = doctorScheduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDoctorSchedules()
    {
        var schedules = await _doctorScheduleService.GetAllDoctorSchedulesAsync();
        return Ok(ApiResponse<IEnumerable<DoctorScheduleDto>>.Ok(schedules));
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetDoctorSchedulesByDoctorId(int doctorId)
    {
        var schedules = await _doctorScheduleService.GetDoctorSchedulesByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<DoctorScheduleDto>>.Ok(schedules));
    }

    [HttpGet("doctor/{doctorId}/active")]
    public async Task<IActionResult> GetActiveDoctorSchedulesByDoctorId(int doctorId)
    {
        var schedules = await _doctorScheduleService.GetActiveDoctorSchedulesByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<DoctorScheduleDto>>.Ok(schedules));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctorScheduleById(int id)
    {
        var schedule = await _doctorScheduleService.GetDoctorScheduleByIdAsync(id);
        if (schedule == null)
        {
            return NotFound(ApiResponse<DoctorScheduleDto>.Fail($"DoctorSchedule with ID {id} not found."));
        }
        return Ok(ApiResponse<DoctorScheduleDto>.Ok(schedule));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateDoctorSchedule([FromBody] CreateDoctorScheduleRequest request)
    {
        // Check for conflicts
        var hasConflict = await _doctorScheduleService.HasConflictAsync(
            request.DoctorId,
            request.ShiftId,
            request.StartDate,
            request.EndDate,
            request.DayOfWeek
        );
        if (hasConflict)
        {
            return BadRequest(ApiResponse<DoctorScheduleDto>.Fail("Schedule conflict detected. Please check the doctor's existing schedules."));
        }

        var schedule = await _doctorScheduleService.CreateDoctorScheduleAsync(request);
        return CreatedAtAction(
            nameof(GetDoctorScheduleById),
            new { id = schedule.Id },
            ApiResponse<DoctorScheduleDto>.Ok(schedule)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateDoctorSchedule(int id, [FromBody] UpdateDoctorScheduleRequest request)
    {
        // Check for conflicts (excluding current schedule)
        var existingSchedule = await _doctorScheduleService.GetDoctorScheduleByIdAsync(id);
        if (existingSchedule == null)
        {
            return NotFound(ApiResponse<DoctorScheduleDto>.Fail($"DoctorSchedule with ID {id} not found."));
        }

        var hasConflict = await _doctorScheduleService.HasConflictAsync(
            existingSchedule.DoctorId,
            request.ShiftId,
            request.StartDate,
            request.EndDate,
            request.DayOfWeek,
            id
        );
        if (hasConflict)
        {
            return BadRequest(ApiResponse<DoctorScheduleDto>.Fail("Schedule conflict detected. Please check the doctor's existing schedules."));
        }

        var schedule = await _doctorScheduleService.UpdateDoctorScheduleAsync(id, request);
        return Ok(ApiResponse<DoctorScheduleDto>.Ok(schedule));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteDoctorSchedule(int id)
    {
        var result = await _doctorScheduleService.DeleteDoctorScheduleAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<DoctorScheduleDto>.Fail($"DoctorSchedule with ID {id} not found."));
        }
        return Ok(ApiResponse<DoctorScheduleDto>.Ok(null, "DoctorSchedule deleted successfully"));
    }
}

