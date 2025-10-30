using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.BannerDto;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Banners;
using VisionCare.WebAPI.Utils;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BannerController : ControllerBase
{
    private readonly IBannerService _service;
    private readonly IS3StorageService _storage;

    public BannerController(IBannerService service, IS3StorageService storage)
    {
        _service = service;
        _storage = storage;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string place = "home_hero")
    {
        // Current schema doesn't include 'place', so we ignore it for now.
        var list = await _service.GetByPlaceAsync(place);
        return Ok(list);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromForm] CreateBannerRequest request,
        IFormFile? image,
        [FromQuery] string place = "home_hero"
    )
    {
        if (image != null && image.Length > 0)
        {
            var url = await _storage.UploadAsync(
                image.OpenReadStream(),
                image.FileName,
                image.ContentType,
                UploadPrefixes.Banner(place)
            );
            request.ImageUrl = url;
        }
        await _service.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { place = "home_hero" }, null);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromForm] UpdateBannerRequest request,
        IFormFile? image,
        [FromQuery] string place = "home_hero"
    )
    {
        if (image != null && image.Length > 0)
        {
            // delete old if exists
            var current = await _service.GetByIdAsync(id);
            var oldKey = VisionCare.WebAPI.Utils.S3KeyHelper.TryExtractObjectKey(current?.ImageUrl);
            var url = await _storage.UploadAsync(
                image.OpenReadStream(),
                image.FileName,
                image.ContentType,
                UploadPrefixes.Banner(place)
            );
            request.ImageUrl = url;
            if (!string.IsNullOrWhiteSpace(oldKey))
            {
                await _storage.DeleteAsync(oldKey!);
            }
        }
        await _service.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
