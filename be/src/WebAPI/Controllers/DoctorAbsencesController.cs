using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAdmin")]
public class DoctorAbsencesController : ControllerBase
{
    private readonly IDoctorAbsenceService _absenceService;

    public DoctorAbsencesController(IDoctorAbsenceService absenceService)
    {
        _absenceService = absenceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorAbsenceDto>>> GetAll()
    {
        var absences = await _absenceService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<DoctorAbsenceDto>>.Ok(absences));
    }

    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<DoctorAbsenceDto>>> GetPending()
    {
        var absences = await _absenceService.GetPendingAsync();
        return Ok(ApiResponse<IEnumerable<DoctorAbsenceDto>>.Ok(absences));
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<ActionResult<IEnumerable<DoctorAbsenceDto>>> GetByDoctor(int doctorId)
    {
        var absences = await _absenceService.GetByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<DoctorAbsenceDto>>.Ok(absences));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorAbsenceDto>> GetById(int id)
    {
        var absence = await _absenceService.GetByIdAsync(id);
        if (absence == null)
        {
            return NotFound(
                ApiResponse<DoctorAbsenceDto>.Fail($"Doctor absence with ID {id} not found.")
            );
        }
        return Ok(ApiResponse<DoctorAbsenceDto>.Ok(absence));
    }

    [HttpPost]
    public async Task<ActionResult<DoctorAbsenceDto>> Create(
        [FromBody] CreateDoctorAbsenceRequest request
    )
    {
        try
        {
            var absence = await _absenceService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = absence.Id },
                ApiResponse<DoctorAbsenceDto>.Ok(absence)
            );
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<DoctorAbsenceDto>.Fail(ex.Message));
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<DoctorAbsenceDto>.Fail(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DoctorAbsenceDto>> Update(
        int id,
        [FromBody] UpdateDoctorAbsenceRequest request
    )
    {
        try
        {
            var absence = await _absenceService.UpdateAsync(id, request);
            return Ok(ApiResponse<DoctorAbsenceDto>.Ok(absence));
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<DoctorAbsenceDto>.Fail(ex.Message));
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<DoctorAbsenceDto>.Fail(ex.Message));
        }
    }

    [HttpPost("{id}/approve")]
    public async Task<ActionResult<DoctorAbsenceDto>> Approve(int id)
    {
        try
        {
            var absence = await _absenceService.ApproveAsync(id);
            return Ok(
                ApiResponse<DoctorAbsenceDto>.Ok(
                    absence,
                    "Absence approved and appointments handled."
                )
            );
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<DoctorAbsenceDto>.Fail(ex.Message));
        }
    }

    [HttpPost("{id}/reject")]
    public async Task<ActionResult<DoctorAbsenceDto>> Reject(int id)
    {
        try
        {
            var absence = await _absenceService.RejectAsync(id);
            return Ok(ApiResponse<DoctorAbsenceDto>.Ok(absence));
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<DoctorAbsenceDto>.Fail(ex.Message));
        }
    }

    [HttpPost("{id}/handle-appointments")]
    public async Task<ActionResult<Dictionary<string, int>>> HandleAppointments(
        int id,
        [FromBody] HandleAbsenceAppointmentsRequest request
    )
    {
        try
        {
            var result = await _absenceService.HandleAbsenceAppointmentsAsync(id, request);
            return Ok(
                ApiResponse<Dictionary<string, int>>.Ok(
                    result,
                    $"Handled {result["total"]} appointments: {result["transferred"]} transferred, {result["cancelled"]} cancelled."
                )
            );
        }
        catch (VisionCare.Application.Exceptions.NotFoundException ex)
        {
            return NotFound(ApiResponse<Dictionary<string, int>>.Fail(ex.Message));
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<Dictionary<string, int>>.Fail(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _absenceService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.Fail($"Doctor absence with ID {id} not found."));
        }
        return NoContent();
    }
}
