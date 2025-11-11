using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.FollowUpDto;
using VisionCare.Application.Interfaces.FollowUp;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAdmin")]
public class FollowUpController : ControllerBase
{
    private readonly IFollowUpService _followUpService;

    public FollowUpController(IFollowUpService followUpService)
    {
        _followUpService = followUpService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFollowUps()
    {
        var followUps = await _followUpService.GetAllFollowUpsAsync();
        return Ok(ApiResponse<IEnumerable<FollowUpDto>>.Ok(followUps));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFollowUpById(int id)
    {
        var followUp = await _followUpService.GetFollowUpByIdAsync(id);
        if (followUp == null)
        {
            return NotFound(ApiResponse<FollowUpDto>.Fail($"Follow-up with ID {id} not found."));
        }
        return Ok(ApiResponse<FollowUpDto>.Ok(followUp));
    }

    [HttpGet("appointment/{appointmentId}")]
    public async Task<IActionResult> GetFollowUpByAppointment(int appointmentId)
    {
        var followUp = await _followUpService.GetFollowUpByAppointmentIdAsync(appointmentId);
        if (followUp == null)
        {
            return NotFound(
                ApiResponse<FollowUpDto>.Fail(
                    $"Follow-up for appointment {appointmentId} not found."
                )
            );
        }
        return Ok(ApiResponse<FollowUpDto>.Ok(followUp));
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetFollowUpsByPatient(int patientId)
    {
        var followUps = await _followUpService.GetFollowUpsByPatientIdAsync(patientId);
        return Ok(ApiResponse<IEnumerable<FollowUpDto>>.Ok(followUps));
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetFollowUpsByDoctor(int doctorId)
    {
        var followUps = await _followUpService.GetFollowUpsByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<FollowUpDto>>.Ok(followUps));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFollowUps(
        [FromQuery] int? patientId,
        [FromQuery] int? doctorId,
        [FromQuery] string? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool desc = false
    )
    {
        var request = new FollowUpSearchRequest
        {
            PatientId = patientId,
            DoctorId = doctorId,
            Status = status,
            FromDate = fromDate,
            ToDate = toDate,
            Page = page,
            PageSize = pageSize,
        };

        var result = await _followUpService.SearchFollowUpsAsync(request);
        return Ok(PagedResponse<FollowUpDto>.Ok(result.items, result.totalCount, page, pageSize));
    }

    [HttpPost]
    public async Task<IActionResult> CreateFollowUp([FromBody] CreateFollowUpRequest request)
    {
        var followUp = await _followUpService.CreateFollowUpAsync(request);
        return CreatedAtAction(
            nameof(GetFollowUpById),
            new { id = followUp.Id },
            ApiResponse<FollowUpDto>.Ok(followUp)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFollowUp(
        int id,
        [FromBody] UpdateFollowUpRequest request
    )
    {
        var followUp = await _followUpService.UpdateFollowUpAsync(id, request);
        return Ok(ApiResponse<FollowUpDto>.Ok(followUp));
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteFollowUp(int id)
    {
        var followUp = await _followUpService.CompleteFollowUpAsync(id);
        return Ok(ApiResponse<FollowUpDto>.Ok(followUp));
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelFollowUp(int id, [FromBody] string? reason = null)
    {
        var followUp = await _followUpService.CancelFollowUpAsync(id, reason);
        return Ok(ApiResponse<FollowUpDto>.Ok(followUp));
    }

    [HttpPut("{id}/reschedule")]
    public async Task<IActionResult> RescheduleFollowUp(int id, [FromBody] DateTime newDate)
    {
        var followUp = await _followUpService.RescheduleFollowUpAsync(id, newDate);
        return Ok(ApiResponse<FollowUpDto>.Ok(followUp));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFollowUp(int id)
    {
        var result = await _followUpService.DeleteFollowUpAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<FollowUpDto>.Fail($"Follow-up with ID {id} not found."));
        }
        return Ok(ApiResponse<FollowUpDto>.Ok(null, "Follow-up deleted successfully"));
    }
}
