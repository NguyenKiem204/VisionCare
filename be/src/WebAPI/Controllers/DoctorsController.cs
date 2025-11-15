using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.DoctorDto;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Doctors;
using VisionCare.WebAPI.Responses;
using VisionCare.WebAPI.Utils;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IS3StorageService _storage;

    public DoctorsController(IDoctorService doctorService, IS3StorageService storage)
    {
        _doctorService = doctorService;
        _storage = storage;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
    {
        var doctors = await _doctorService.GetAllDoctorsAsync();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        if (doctor == null)
        {
            return NotFound();
        }
        return Ok(doctor);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> CreateDoctor(CreateDoctorRequest request)
    {
        var doctor = await _doctorService.CreateDoctorAsync(request);
        return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DoctorDto>> UpdateDoctor(
        int id,
        [FromForm] UpdateDoctorRequest request,
        IFormFile? avatar
    )
    {
        try
        {
            var currentDoctor = await _doctorService.GetDoctorByIdAsync(id);
            if (currentDoctor == null)
            {
                return NotFound();
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
                return BadRequest(string.Join("; ", errors));
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
                    UploadPrefixes.DoctorAvatar(id)
                );
                request.Avatar = url;
                request.ProfileImage = url;
            }
            else
            {
                // Giữ nguyên ảnh cũ nếu không có ảnh mới
                request.Avatar = currentDoctor.Avatar ?? currentDoctor.ProfileImage;
                request.ProfileImage = currentDoctor.ProfileImage ?? currentDoctor.Avatar;
            }

            var doctor = await _doctorService.UpdateDoctorAsync(id, request);
            return Ok(doctor);
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDoctor(int id)
    {
        var result = await _doctorService.DeleteDoctorAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("specialization/{specializationId}")]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsBySpecialization(
        int specializationId
    )
    {
        var doctors = await _doctorService.GetDoctorsBySpecializationAsync(specializationId);
        return Ok(doctors);
    }

    [HttpGet("service/{serviceDetailId}")]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsByService(
        int serviceDetailId
    )
    {
        var doctors = await _doctorService.GetDoctorsByServiceAsync(serviceDetailId);
        return Ok(doctors);
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResponse<DoctorDto>>> SearchDoctors(
        [FromQuery] string? keyword,
        [FromQuery] int? specializationId,
        [FromQuery] double? minRating,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "id",
        [FromQuery] bool desc = false
    )
    {
        var (doctors, totalCount) = await _doctorService.SearchDoctorsAsync(
            keyword ?? string.Empty, 
            specializationId, 
            minRating, 
            page, 
            pageSize, 
            sortBy, 
            desc
        );
        
        var response = new PagedResponse<DoctorDto>
        {
            Items = doctors,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
        
        return Ok(response);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAvailableDoctors(
        [FromQuery] DateTime date
    )
    {
        var doctors = await _doctorService.GetAvailableDoctorsAsync(date);
        return Ok(doctors);
    }

    [HttpPut("{id}/rating")]
    public async Task<ActionResult<DoctorDto>> UpdateDoctorRating(int id, [FromBody] double rating)
    {
        var doctor = await _doctorService.UpdateDoctorRatingAsync(id, rating);
        return Ok(doctor);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<DoctorDto>> UpdateDoctorStatus(int id, [FromBody] string status)
    {
        var doctor = await _doctorService.UpdateDoctorStatusAsync(id, status);
        return Ok(doctor);
    }

    [HttpGet("top-rated")]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetTopRatedDoctors(
        [FromQuery] int count = 5
    )
    {
        var doctors = await _doctorService.GetTopRatedDoctorsAsync(count);
        return Ok(doctors);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult<object>> GetDoctorStatistics()
    {
        var totalCount = await _doctorService.GetTotalDoctorsCountAsync();
        var averageRating = await _doctorService.GetAverageRatingAsync();

        return Ok(new { TotalCount = totalCount, AverageRating = averageRating });
    }
}
