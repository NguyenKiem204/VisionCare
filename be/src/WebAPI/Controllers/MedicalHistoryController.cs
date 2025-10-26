using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.MedicalHistoryDto;
using VisionCare.Application.Interfaces.MedicalHistory;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAdmin")]
public class MedicalHistoryController : ControllerBase
{
    private readonly IMedicalHistoryService _medicalHistoryService;

    public MedicalHistoryController(IMedicalHistoryService medicalHistoryService)
    {
        _medicalHistoryService = medicalHistoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMedicalHistories()
    {
        var medicalHistories = await _medicalHistoryService.GetAllMedicalHistoriesAsync();
        return Ok(ApiResponse<IEnumerable<MedicalHistoryDto>>.Ok(medicalHistories));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMedicalHistoryById(int id)
    {
        var medicalHistory = await _medicalHistoryService.GetMedicalHistoryByIdAsync(id);
        if (medicalHistory == null)
        {
            return NotFound(ApiResponse<MedicalHistoryDto>.Fail($"Medical history with ID {id} not found."));
        }
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(medicalHistory));
    }

    [HttpGet("appointment/{appointmentId}")]
    public async Task<IActionResult> GetMedicalHistoryByAppointment(int appointmentId)
    {
        var medicalHistory = await _medicalHistoryService.GetMedicalHistoryByAppointmentIdAsync(appointmentId);
        if (medicalHistory == null)
        {
            return NotFound(ApiResponse<MedicalHistoryDto>.Fail($"Medical history for appointment {appointmentId} not found."));
        }
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(medicalHistory));
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetMedicalHistoriesByPatient(int patientId)
    {
        var medicalHistories = await _medicalHistoryService.GetMedicalHistoriesByPatientIdAsync(patientId);
        return Ok(ApiResponse<IEnumerable<MedicalHistoryDto>>.Ok(medicalHistories));
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetMedicalHistoriesByDoctor(int doctorId)
    {
        var medicalHistories = await _medicalHistoryService.GetMedicalHistoriesByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<MedicalHistoryDto>>.Ok(medicalHistories));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchMedicalHistories(
        [FromQuery] string? keyword,
        [FromQuery] int? patientId,
        [FromQuery] int? doctorId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool desc = false
    )
    {
        var request = new MedicalHistorySearchRequest
        {
            Keyword = keyword,
            PatientId = patientId,
            DoctorId = doctorId,
            FromDate = fromDate,
            ToDate = toDate,
            Page = page,
            PageSize = pageSize,
            SortBy = sortBy,
            Desc = desc
        };

        var result = await _medicalHistoryService.SearchMedicalHistoriesAsync(request);
        return Ok(PagedResponse<MedicalHistoryDto>.Ok(result.items, result.totalCount, page, pageSize));
    }

    [HttpPost]
    public async Task<IActionResult> CreateMedicalHistory([FromBody] CreateMedicalHistoryRequest request)
    {
        var medicalHistory = await _medicalHistoryService.CreateMedicalHistoryAsync(request);
        return CreatedAtAction(nameof(GetMedicalHistoryById), new { id = medicalHistory.Id }, ApiResponse<MedicalHistoryDto>.Ok(medicalHistory));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMedicalHistory(int id, [FromBody] UpdateMedicalHistoryRequest request)
    {
        var medicalHistory = await _medicalHistoryService.UpdateMedicalHistoryAsync(id, request);
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(medicalHistory));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedicalHistory(int id)
    {
        var result = await _medicalHistoryService.DeleteMedicalHistoryAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<MedicalHistoryDto>.Fail($"Medical history with ID {id} not found."));
        }
        return Ok(ApiResponse<MedicalHistoryDto>.Ok(null, "Medical history deleted successfully"));
    }
}
