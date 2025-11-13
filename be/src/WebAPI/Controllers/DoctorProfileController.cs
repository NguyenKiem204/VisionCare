using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.DoctorCertificateDto;
using VisionCare.Application.DTOs.DoctorDegreeDto;
using VisionCare.Application.DTOs.DoctorDto;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.DoctorCertificates;
using VisionCare.Application.Interfaces.DoctorDegrees;
using VisionCare.Application.Interfaces.Doctors;
using VisionCare.WebAPI.Responses;
using VisionCare.WebAPI.Utils;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/doctor/me")]
[Authorize(Policy = "DoctorOnly")]
public class DoctorProfileController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IDoctorCertificateService _doctorCertificateService;
    private readonly IDoctorDegreeService _doctorDegreeService;
    private readonly IS3StorageService _storage;

    public DoctorProfileController(
        IDoctorService doctorService,
        IDoctorCertificateService doctorCertificateService,
        IDoctorDegreeService doctorDegreeService,
        IS3StorageService storage
    )
    {
        _doctorService = doctorService;
        _doctorCertificateService = doctorCertificateService;
        _doctorDegreeService = doctorDegreeService;
        _storage = storage;
    }

    private int GetCurrentAccountId()
    {
        var idClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
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
    public async Task<ActionResult<DoctorDto>> UpdateMyProfile(
        [FromForm] UpdateDoctorRequest request,
        IFormFile? avatar
    )
    {
        try
        {
            var doctorId = GetCurrentAccountId();
            var currentDoctor = await _doctorService.GetDoctorByIdAsync(doctorId);
            if (currentDoctor == null)
            {
                return NotFound(ApiResponse<DoctorDto>.Fail("Doctor profile not found."));
            }

            var formCollection = Request.HasFormContentType ? Request.Form : null;

            if (formCollection != null)
            {
                if (
                    formCollection.TryGetValue("doctorName", out var doctorNameValue)
                    && !string.IsNullOrWhiteSpace(doctorNameValue)
                )
                {
                    request.DoctorName = doctorNameValue!;
                }

                if (
                    formCollection.TryGetValue("dob", out var dobValue)
                    && !string.IsNullOrWhiteSpace(dobValue)
                    && DateOnly.TryParse(dobValue, out var parsedDob)
                )
                {
                    request.Dob = parsedDob;
                }

                if (
                    formCollection.TryGetValue("experienceYears", out var expValue)
                    && !string.IsNullOrWhiteSpace(expValue)
                    && int.TryParse(expValue, out var parsedExp)
                )
                {
                    request.ExperienceYears = parsedExp;
                }

                if (
                    formCollection.TryGetValue("specializationId", out var specValue)
                    && !string.IsNullOrWhiteSpace(specValue)
                    && int.TryParse(specValue, out var parsedSpec)
                )
                {
                    request.SpecializationId = parsedSpec;
                }
            }

            if (string.IsNullOrWhiteSpace(request.DoctorName))
            {
                request.DoctorName = currentDoctor.DoctorName;
            }

            ModelState.Clear();
            if (!TryValidateModel(request))
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => e.ErrorMessage))
                    .ToList();
                return BadRequest(ApiResponse<DoctorDto>.Fail(string.Join("; ", errors)));
            }

            if (avatar != null && avatar.Length > 0)
            {
                var oldKey = S3KeyHelper.TryExtractObjectKey(
                    currentDoctor.Avatar ?? currentDoctor.ProfileImage
                );
                if (!string.IsNullOrWhiteSpace(oldKey))
                {
                    await _storage.DeleteAsync(oldKey);
                }

                var url = await _storage.UploadAsync(
                    avatar.OpenReadStream(),
                    avatar.FileName,
                    avatar.ContentType,
                    UploadPrefixes.DoctorAvatar(doctorId)
                );
                request.Avatar = url;
                request.ProfileImage = url;
            }

            var updated = await _doctorService.UpdateDoctorAsync(doctorId, request);
            return Ok(ApiResponse<DoctorDto>.Ok(updated));
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<DoctorDto>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<DoctorDto>.Fail($"An error occurred: {ex.Message}"));
        }
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
        [FromForm] CreateDoctorCertificateRequest request,
        IFormFile? certificateImage
    )
    {
        try
        {
            var doctorId = GetCurrentAccountId();

            // Parse DateOnly from FormData if provided
            if (
                Request.Form.TryGetValue("issuedDate", out var issuedDateValue)
                && !string.IsNullOrWhiteSpace(issuedDateValue)
            )
            {
                if (DateOnly.TryParse(issuedDateValue, out var parsedIssuedDate))
                {
                    request.IssuedDate = parsedIssuedDate;
                }
            }

            if (
                Request.Form.TryGetValue("expiryDate", out var expiryDateValue)
                && !string.IsNullOrWhiteSpace(expiryDateValue)
            )
            {
                if (DateOnly.TryParse(expiryDateValue, out var parsedExpiryDate))
                {
                    request.ExpiryDate = parsedExpiryDate;
                }
            }

            // Upload certificate image to S3 if provided
            if (certificateImage != null && certificateImage.Length > 0)
            {
                var url = await _storage.UploadAsync(
                    certificateImage.OpenReadStream(),
                    certificateImage.FileName,
                    certificateImage.ContentType,
                    UploadPrefixes.DoctorCertificate(doctorId, request.CertificateId)
                );
                request.CertificateImage = url;
            }

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
            return CreatedAtAction(
                nameof(GetMyCertificates),
                ApiResponse<DoctorCertificateDto>.Ok(created)
            );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                ApiResponse<DoctorCertificateDto>.Fail($"An error occurred: {ex.Message}")
            );
        }
    }

    [HttpDelete("certificates/{certificateId}")]
    public async Task<ActionResult> DeleteMyCertificate(int certificateId)
    {
        var doctorId = GetCurrentAccountId();
        var list = await _doctorCertificateService.GetCertificatesByDoctorAsync(doctorId);
        var item = list.FirstOrDefault(c => c.CertificateId == certificateId);
        if (item == null)
        {
            return NotFound();
        }
        var ok = await _doctorCertificateService.DeleteDoctorCertificateAsync(item.Id);
        if (!ok)
            return NotFound();
        return NoContent();
    }

    [HttpGet("degrees")]
    public async Task<ActionResult<IEnumerable<DoctorDegreeDto>>> GetMyDegrees()
    {
        var doctorId = GetCurrentAccountId();
        var list = await _doctorDegreeService.GetDegreesByDoctorAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<DoctorDegreeDto>>.Ok(list));
    }

    [HttpPost("degrees")]
    public async Task<ActionResult<DoctorDegreeDto>> CreateMyDegree(
        [FromForm] CreateDoctorDegreeRequest request,
        IFormFile? certificateImage
    )
    {
        try
        {
            var doctorId = GetCurrentAccountId();

            // Parse DateOnly from FormData if provided
            if (
                Request.Form.TryGetValue("issuedDate", out var issuedDateValue)
                && !string.IsNullOrWhiteSpace(issuedDateValue)
            )
            {
                if (DateOnly.TryParse(issuedDateValue, out var parsedIssuedDate))
                {
                    request.IssuedDate = parsedIssuedDate;
                }
            }

            // Upload certificate image to S3 if provided
            if (certificateImage != null && certificateImage.Length > 0)
            {
                var url = await _storage.UploadAsync(
                    certificateImage.OpenReadStream(),
                    certificateImage.FileName,
                    certificateImage.ContentType,
                    UploadPrefixes.DoctorDegree(doctorId, request.DegreeId)
                );
                request.CertificateImage = url;
            }

            var payload = new CreateDoctorDegreeRequest
            {
                DoctorId = doctorId,
                DegreeId = request.DegreeId,
                IssuedDate = request.IssuedDate,
                IssuedBy = request.IssuedBy,
                CertificateImage = request.CertificateImage,
                Status = string.IsNullOrWhiteSpace(request.Status) ? "Active" : request.Status,
            };

            var created = await _doctorDegreeService.CreateDoctorDegreeAsync(payload);
            return CreatedAtAction(nameof(GetMyDegrees), ApiResponse<DoctorDegreeDto>.Ok(created));
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                ApiResponse<DoctorDegreeDto>.Fail($"An error occurred: {ex.Message}")
            );
        }
    }

    [HttpDelete("degrees/{degreeId}")]
    public async Task<ActionResult> DeleteMyDegree(int degreeId)
    {
        var doctorId = GetCurrentAccountId();
        var list = await _doctorDegreeService.GetDegreesByDoctorAsync(doctorId);
        var item = list.FirstOrDefault(d => d.DegreeId == degreeId);
        if (item == null)
        {
            return NotFound();
        }
        var ok = await _doctorDegreeService.DeleteDoctorDegreeAsync(item.Id);
        if (!ok)
            return NotFound();
        return NoContent();
    }
}
