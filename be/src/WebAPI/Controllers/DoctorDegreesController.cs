using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.DoctorDegreeDto;
using VisionCare.Application.Interfaces.DoctorDegrees;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorDegreesController : ControllerBase
{
    private readonly IDoctorDegreeService _doctorDegreeService;

    public DoctorDegreesController(IDoctorDegreeService doctorDegreeService)
    {
        _doctorDegreeService = doctorDegreeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDegreeDto>>> GetDoctorDegrees()
    {
        var doctorDegrees = await _doctorDegreeService.GetAllDoctorDegreesAsync();
        return Ok(doctorDegrees);
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<ActionResult<IEnumerable<DoctorDegreeDto>>> GetDegreesByDoctor(int doctorId)
    {
        var degrees = await _doctorDegreeService.GetDegreesByDoctorAsync(doctorId);
        return Ok(degrees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorDegreeDto>> GetDoctorDegree(int id)
    {
        var doctorDegree = await _doctorDegreeService.GetDoctorDegreeByIdAsync(id);
        if (doctorDegree == null)
        {
            return NotFound();
        }
        return Ok(doctorDegree);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDegreeDto>> CreateDoctorDegree(CreateDoctorDegreeRequest request)
    {
        var doctorDegree = await _doctorDegreeService.CreateDoctorDegreeAsync(request);
        return CreatedAtAction(nameof(GetDoctorDegree), new { id = doctorDegree.Id }, doctorDegree);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DoctorDegreeDto>> UpdateDoctorDegree(int id, UpdateDoctorDegreeRequest request)
    {
        var doctorDegree = await _doctorDegreeService.UpdateDoctorDegreeAsync(id, request);
        return Ok(doctorDegree);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDoctorDegree(int id)
    {
        var result = await _doctorDegreeService.DeleteDoctorDegreeAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
