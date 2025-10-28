using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.CertificateDto;
using VisionCare.Application.Interfaces.Certificates;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificatesController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CertificateDto>>> GetCertificates()
    {
        var certificates = await _certificateService.GetAllCertificatesAsync();
        return Ok(certificates);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CertificateDto>> GetCertificate(int id)
    {
        var certificate = await _certificateService.GetCertificateByIdAsync(id);
        if (certificate == null)
        {
            return NotFound();
        }
        return Ok(certificate);
    }

    [HttpPost]
    public async Task<ActionResult<CertificateDto>> CreateCertificate(CreateCertificateRequest request)
    {
        var certificate = await _certificateService.CreateCertificateAsync(request);
        return CreatedAtAction(nameof(GetCertificate), new { id = certificate.Id }, certificate);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CertificateDto>> UpdateCertificate(int id, UpdateCertificateRequest request)
    {
        var certificate = await _certificateService.UpdateCertificateAsync(id, request);
        return Ok(certificate);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCertificate(int id)
    {
        var result = await _certificateService.DeleteCertificateAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
