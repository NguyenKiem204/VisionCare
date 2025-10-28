using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.DoctorCertificateDto;
using VisionCare.Application.Interfaces.DoctorCertificates;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorCertificatesController : ControllerBase
{
    private readonly IDoctorCertificateService _doctorCertificateService;

    public DoctorCertificatesController(IDoctorCertificateService doctorCertificateService)
    {
        _doctorCertificateService = doctorCertificateService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorCertificateDto>>> GetDoctorCertificates()
    {
        var doctorCertificates = await _doctorCertificateService.GetAllDoctorCertificatesAsync();
        return Ok(doctorCertificates);
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<ActionResult<IEnumerable<DoctorCertificateDto>>> GetCertificatesByDoctor(int doctorId)
    {
        var certificates = await _doctorCertificateService.GetCertificatesByDoctorAsync(doctorId);
        return Ok(certificates);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorCertificateDto>> GetDoctorCertificate(int id)
    {
        var doctorCertificate = await _doctorCertificateService.GetDoctorCertificateByIdAsync(id);
        if (doctorCertificate == null)
        {
            return NotFound();
        }
        return Ok(doctorCertificate);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorCertificateDto>> CreateDoctorCertificate(CreateDoctorCertificateRequest request)
    {
        var doctorCertificate = await _doctorCertificateService.CreateDoctorCertificateAsync(request);
        return CreatedAtAction(nameof(GetDoctorCertificate), new { id = doctorCertificate.Id }, doctorCertificate);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DoctorCertificateDto>> UpdateDoctorCertificate(int id, UpdateDoctorCertificateRequest request)
    {
        var doctorCertificate = await _doctorCertificateService.UpdateDoctorCertificateAsync(id, request);
        return Ok(doctorCertificate);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteDoctorCertificate(int id)
    {
        var result = await _doctorCertificateService.DeleteDoctorCertificateAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
