using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.Interfaces;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
    private readonly IS3StorageService _storage;

    public StorageController(IS3StorageService storage)
    {
        _storage = storage;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Upload(
        [FromQuery] string prefix,
        IFormFile file,
        CancellationToken ct
    )
    {
        if (string.IsNullOrWhiteSpace(prefix))
            prefix = "misc";
        if (file == null)
            return BadRequest("File is required");
        var url = await _storage.UploadAsync(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            prefix,
            ct
        );
        return Ok(new { url });
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromQuery] string key, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest("key is required");
        await _storage.DeleteAsync(key, ct);
        return NoContent();
    }
}
