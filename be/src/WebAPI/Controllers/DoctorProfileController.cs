using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VisionCare.Application.DTOs.DoctorDto;
using VisionCare.Application.DTOs.DoctorCertificateDto;
using VisionCare.Application.Interfaces.DoctorCertificates;
using VisionCare.Application.Interfaces.Doctors;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/doctor/me")]
[Authorize(Policy = "DoctorOnly")]
public class DoctorProfileController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IDoctorCertificateService _doctorCertificateService;

    public DoctorProfileController(
        IDoctorService doctorService,
        IDoctorCertificateService doctorCertificateService
    )
    {
        _doctorService = doctorService;
        _doctorCertificateService = doctorCertificateService;
    }

    private int GetCurrentAccountId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("account_id")?.Value
            ?? User.FindFirst("sub")?.Value;
        if (!int.TryParse(idClaim, out var accountId))
        {
            throw new UnauthorizedAccessException("Invalid or missing account id claim.");
        }
        return accountId;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<DoctorDto>> GetMyProfile()
    {
        var doctorId = GetCurrentAccountId();
        var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
        if (doctor == null)
        {
            return NotFound(ApiResponse<DoctorDto>.Fail("Doctor profile not found."));
        }
        return Ok(ApiResponse<DoctorDto>.Ok(doctor));
    }

    [HttpPut("profile")]
    public async Task<ActionResult<DoctorDto>> UpdateMyProfile([FromBody] UpdateDoctorRequest request)
    {
        var doctorId = GetCurrentAccountId();
        var updated = await _doctorService.UpdateDoctorAsync(doctorId, request);
        return Ok(ApiResponse<DoctorDto>.Ok(updated));
    }

    [HttpGet("certificates")]
    public async Task<ActionResult<IEnumerable<DoctorCertificateDto>>> GetMyCertificates()
    {
        var doctorId = GetCurrentAccountId();
        var list = await _doctorCertificateService.GetCertificatesByDoctorAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<DoctorCertificateDto>>.Ok(list));
    }

    [HttpPost("certificates")]
    public async Task<ActionResult<DoctorCertificateDto>> CreateMyCertificate(
        [FromBody] CreateDoctorCertificateRequest request
    )
    {
        var doctorId = GetCurrentAccountId();
        var payload = new CreateDoctorCertificateRequest
        {
            DoctorId = doctorId,
            CertificateId = request.CertificateId,
            IssuedDate = request.IssuedDate,
            IssuedBy = request.IssuedBy,
            CertificateImage = request.CertificateImage,
            ExpiryDate = request.ExpiryDate,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Active" : request.Status,
        };

        var created = await _doctorCertificateService.CreateDoctorCertificateAsync(payload);
        return CreatedAtAction(nameof(GetMyCertificates), ApiResponse<DoctorCertificateDto>.Ok(created));
    }

    [HttpDelete("certificates/{certificateId}")]
    public async Task<ActionResult> DeleteMyCertificate(int certificateId)
    {
        var doctorId = GetCurrentAccountId();
        // Service delete by composite key may not exist; fallback to list and delete by id
        var list = await _doctorCertificateService.GetCertificatesByDoctorAsync(doctorId);
        var item = list.FirstOrDefault(c => c.CertificateId == certificateId);
        if (item == null)
        {
            return NotFound();
        }
        var ok = await _doctorCertificateService.DeleteDoctorCertificateAsync(item.Id);
        if (!ok) return NotFound();
        return NoContent();
    }
}


