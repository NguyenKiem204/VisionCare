using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.ScheduleDto;
using VisionCare.Application.DTOs.SlotDto;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulingController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public SchedulingController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    #region Slot Management

    
    [HttpGet("slots")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetAllSlots()
    {
        var slots = await _scheduleService.GetAllSlotsAsync();
        return Ok(ApiResponse<IEnumerable<SlotDto>>.Ok(slots));
    }

    [HttpGet("slots/{id}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetSlotById(int id)
    {
        var slot = await _scheduleService.GetSlotByIdAsync(id);
        if (slot == null)
        {
            return NotFound(ApiResponse<SlotDto>.Fail($"Slot with ID {id} not found."));
        }
        return Ok(ApiResponse<SlotDto>.Ok(slot));
    }

    [HttpGet("slots/service-type/{serviceTypeId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetSlotsByServiceType(int serviceTypeId)
    {
        var slots = await _scheduleService.GetSlotsByServiceTypeAsync(serviceTypeId);
        return Ok(ApiResponse<IEnumerable<SlotDto>>.Ok(slots));
    }

    [HttpPost("slots")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateSlot([FromBody] CreateSlotRequest request)
    {
        var slot = await _scheduleService.CreateSlotAsync(request);
        return CreatedAtAction(
            nameof(GetSlotById),
            new { id = slot.Id },
            ApiResponse<SlotDto>.Ok(slot)
        );
    }

    [HttpPut("slots/{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateSlot(int id, [FromBody] UpdateSlotRequest request)
    {
        var slot = await _scheduleService.UpdateSlotAsync(id, request);
        return Ok(ApiResponse<SlotDto>.Ok(slot));
    }

    [HttpDelete("slots/{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteSlot(int id)
    {
        var result = await _scheduleService.DeleteSlotAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<SlotDto>.Fail($"Slot with ID {id} not found."));
        }
        return Ok(ApiResponse<SlotDto>.Ok(null, "Slot deleted successfully"));
    }

    #endregion

    #region Schedule Management

    [HttpGet("schedules")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetAllSchedules()
    {
        var schedules = await _scheduleService.GetAllSchedulesAsync();
        return Ok(ApiResponse<IEnumerable<ScheduleDto>>.Ok(schedules));
    }

    [HttpGet("schedules/{id}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetScheduleById(int id)
    {
        var schedule = await _scheduleService.GetScheduleByIdAsync(id);
        if (schedule == null)
        {
            return NotFound(ApiResponse<ScheduleDto>.Fail($"Schedule with ID {id} not found."));
        }
        return Ok(ApiResponse<ScheduleDto>.Ok(schedule));
    }

    [HttpGet("schedules/doctor/{doctorId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetSchedulesByDoctor(int doctorId)
    {
        var schedules = await _scheduleService.GetSchedulesByDoctorAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<ScheduleDto>>.Ok(schedules));
    }

    [HttpGet("schedules/doctor/{doctorId}/date/{scheduleDate:datetime}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetSchedulesByDoctorAndDate(
        int doctorId,
        DateTime scheduleDate
    )
    {
        var schedules = await _scheduleService.GetSchedulesByDoctorAndDateAsync(
            doctorId,
            DateOnly.FromDateTime(scheduleDate)
        );
        return Ok(ApiResponse<IEnumerable<ScheduleDto>>.Ok(schedules));
    }

    [HttpPost("schedules")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleRequest request)
    {
        var schedule = await _scheduleService.CreateScheduleAsync(request);
        return CreatedAtAction(
            nameof(GetScheduleById),
            new { id = schedule.Id },
            ApiResponse<ScheduleDto>.Ok(schedule)
        );
    }

    [HttpPut("schedules/{id}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> UpdateSchedule(
        int id,
        [FromBody] UpdateScheduleRequest request
    )
    {
        var schedule = await _scheduleService.UpdateScheduleAsync(id, request);
        return Ok(ApiResponse<ScheduleDto>.Ok(schedule));
    }
    
    [HttpDelete("schedules/{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var result = await _scheduleService.DeleteScheduleAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<ScheduleDto>.Fail($"Schedule with ID {id} not found."));
        }
        return Ok(ApiResponse<ScheduleDto>.Ok(null, "Schedule deleted successfully"));
    }

    #endregion

    #region Availability Management

    [HttpPost("available-slots")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetAvailableSlots([FromBody] AvailableSlotsRequest request)
    {
        var slots = await _scheduleService.GetAvailableSlotsAsync(request);
        return Ok(ApiResponse<IEnumerable<ScheduleDto>>.Ok(slots));
    }

    [HttpGet("available-slots/check")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> IsSlotAvailable(
        int doctorId,
        int slotId,
        DateTime scheduleDate
    )
    {
        var isAvailable = await _scheduleService.IsSlotAvailableAsync(
            doctorId,
            slotId,
            DateOnly.FromDateTime(scheduleDate)
        );
        return Ok(ApiResponse<bool>.Ok(isAvailable));
    }

    [HttpPost("book-slot")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> BookSlot(int doctorId, int slotId, DateTime scheduleDate)
    {
        var schedule = await _scheduleService.BookSlotAsync(
            doctorId,
            slotId,
            DateOnly.FromDateTime(scheduleDate)
        );
        return Ok(ApiResponse<ScheduleDto>.Ok(schedule));
    }

    [HttpPost("block-slot")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> BlockSlot(
        int doctorId,
        int slotId,
        DateTime scheduleDate,
        [FromBody] string? reason = null
    )
    {
        var schedule = await _scheduleService.BlockSlotAsync(
            doctorId,
            slotId,
            DateOnly.FromDateTime(scheduleDate),
            reason
        );
        return Ok(ApiResponse<ScheduleDto>.Ok(schedule));
    }

    [HttpPost("unblock-slot")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> UnblockSlot(int doctorId, int slotId, DateTime scheduleDate)
    {
        var schedule = await _scheduleService.UnblockSlotAsync(
            doctorId,
            slotId,
            DateOnly.FromDateTime(scheduleDate)
        );
        return Ok(ApiResponse<ScheduleDto>.Ok(schedule));
    }

    #endregion

    #region Search and Statistics

    [HttpPost("schedules/search")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> SearchSchedules([FromBody] ScheduleSearchRequest request)
    {
        var schedules = await _scheduleService.SearchSchedulesAsync(request);
        return Ok(ApiResponse<IEnumerable<ScheduleDto>>.Ok(schedules));
    }

    [HttpGet("schedules/count")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetTotalSchedulesCount()
    {
        var count = await _scheduleService.GetTotalSchedulesCountAsync();
        return Ok(ApiResponse<int>.Ok(count));
    }

    #endregion
}
