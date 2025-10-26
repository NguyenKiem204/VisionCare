using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.DegreeDto;
using VisionCare.Application.Interfaces.Degrees;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DegreesController : ControllerBase
{
    private readonly IDegreeService _degreeService;

    public DegreesController(IDegreeService degreeService)
    {
        _degreeService = degreeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DegreeDto>>> GetDegrees()
    {
        var degrees = await _degreeService.GetAllDegreesAsync();
        return Ok(degrees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DegreeDto>> GetDegree(int id)
    {
        var degree = await _degreeService.GetDegreeByIdAsync(id);
        if (degree == null)
        {
            return NotFound();
        }
        return Ok(degree);
    }

    [HttpPost]
    public async Task<ActionResult<DegreeDto>> CreateDegree(CreateDegreeRequest request)
    {
        var degree = await _degreeService.CreateDegreeAsync(request);
        return CreatedAtAction(nameof(GetDegree), new { id = degree.Id }, degree);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DegreeDto>> UpdateDegree(int id, UpdateDegreeRequest request)
    {
        var degree = await _degreeService.UpdateDegreeAsync(id, request);
        return Ok(degree);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDegree(int id)
    {
        var result = await _degreeService.DeleteDegreeAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
