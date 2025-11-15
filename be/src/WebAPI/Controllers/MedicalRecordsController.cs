using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.MedicalHistoryDto;
using VisionCare.Application.Interfaces.MedicalHistory;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/medical-records")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IMedicalHistoryService _medicalHistoryService;

    public MedicalRecordsController(IMedicalHistoryService medicalHistoryService)
    {
        _medicalHistoryService = medicalHistoryService;
    }

    [HttpGet]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetAllMedicalHistories()
    {
        var medicalHistories = await _medicalHistoryService.GetAllMedicalHistoriesAsync();
        return Ok(ApiResponse<IEnumerable<MedicalHistoryDto>>.Ok(medicalHistories));
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetMedicalHistoryById(int id)
    {
        var medicalHistory = await _medicalHistoryService.GetMedicalHistoryByIdAsync(id);
        if (medicalHistory == null)
        {
            return NotFound(
                ApiResponse<MedicalHistoryDto>.Fail($"Medical history with ID {id} not found.")
            );
        }
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(medicalHistory));
    }

    [HttpGet("appointment/{appointmentId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetMedicalHistoryByAppointmentId(int appointmentId)
    {
        var medicalHistory = await _medicalHistoryService.GetMedicalHistoryByAppointmentIdAsync(
            appointmentId
        );
        if (medicalHistory == null)
        {
            return NotFound(
                ApiResponse<MedicalHistoryDto>.Fail(
                    $"Medical history for appointment {appointmentId} not found."
                )
            );
        }
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(medicalHistory));
    }

    [HttpGet("patient/{patientId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetMedicalHistoriesByPatientId(int patientId)
    {
        var medicalHistories = await _medicalHistoryService.GetMedicalHistoriesByPatientIdAsync(
            patientId
        );
        return Ok(ApiResponse<IEnumerable<MedicalHistoryDto>>.Ok(medicalHistories));
    }

    [HttpGet("doctor/{doctorId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetMedicalHistoriesByDoctorId(int doctorId)
    {
        var medicalHistories = await _medicalHistoryService.GetMedicalHistoriesByDoctorIdAsync(
            doctorId
        );
        return Ok(ApiResponse<IEnumerable<MedicalHistoryDto>>.Ok(medicalHistories));
    }

    [HttpPost]
    [Authorize(Policy = "DoctorOnly")]
    public async Task<IActionResult> CreateMedicalHistory(
        [FromBody] CreateMedicalHistoryRequest request
    )
    {
        var medicalHistory = await _medicalHistoryService.CreateMedicalHistoryAsync(request);
        return CreatedAtAction(
            nameof(GetMedicalHistoryById),
            new { id = medicalHistory.Id },
            ApiResponse<MedicalHistoryDto>.Ok(medicalHistory)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "DoctorOnly")]
    public async Task<IActionResult> UpdateMedicalHistory(
        int id,
        [FromBody] UpdateMedicalHistoryRequest request
    )
    {
        var medicalHistory = await _medicalHistoryService.UpdateMedicalHistoryAsync(id, request);
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(medicalHistory));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteMedicalHistory(int id)
    {
        var result = await _medicalHistoryService.DeleteMedicalHistoryAsync(id);
        if (!result)
        {
            return NotFound(
                ApiResponse<MedicalHistoryDto>.Fail($"Medical history with ID {id} not found.")
            );
        }
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(null, "Medical history deleted successfully"));
    }

    [HttpPost("search")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> SearchMedicalHistories(
        [FromBody] MedicalHistorySearchRequest request
    )
    {
        var result = await _medicalHistoryService.SearchMedicalHistoriesAsync(request);
        return Ok(
            PagedResponse<MedicalHistoryDto>.Ok(
                result.items,
                result.totalCount,
                request.Page,
                request.PageSize
            )
        );
    }

    [HttpGet("count")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetTotalMedicalHistoriesCount()
    {
        var count = await _medicalHistoryService.GetTotalMedicalHistoriesCountAsync();
        return Ok(ApiResponse<int>.Ok(count));
    }
}
