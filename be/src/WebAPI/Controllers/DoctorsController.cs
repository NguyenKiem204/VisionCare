using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.DoctorDto;
using VisionCare.Application.Interfaces.Doctors;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
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
    public async Task<ActionResult<DoctorDto>> UpdateDoctor(int id, UpdateDoctorRequest request)
    {
        var doctor = await _doctorService.UpdateDoctorAsync(id, request);
        return Ok(doctor);
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
