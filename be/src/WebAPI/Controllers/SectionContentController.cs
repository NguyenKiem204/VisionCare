using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.SectionContentDto;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Content;
using VisionCare.WebAPI.Utils;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionContentController : ControllerBase
{
    private readonly ISectionContentService _service;
    private readonly IS3StorageService _storage;

    public SectionContentController(ISectionContentService service, IS3StorageService storage)
    {
        _service = service;
        _storage = storage;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetByKey([FromRoute] string key)
    {
        var dto = await _service.GetByKeyAsync(key);
        if (dto == null)
            return NotFound();
        return Ok(dto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromForm] SectionContentUpsertDto request,
        IFormFile? image
    )
    {
        if (string.IsNullOrWhiteSpace(request.SectionKey))
            return BadRequest("section_key is required");
        if (image != null && image.Length > 0)
        {
            var url = await _storage.UploadAsync(
                image.OpenReadStream(),
                image.FileName,
                image.ContentType,
                UploadPrefixes.Section(request.SectionKey)
            );
            request.ImageUrl = url;
        }
        await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetByKey), new { key = request.SectionKey }, null);
    }

    [HttpPut("{key}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        [FromRoute] string key,
        [FromForm] SectionContentUpsertDto request,
        IFormFile? image
    )
    {
        var found = await _service.GetByKeyAsync(key);
        if (found == null)
            return NotFound();
        if (image != null && image.Length > 0)
        {
            var oldKey = VisionCare.WebAPI.Utils.S3KeyHelper.TryExtractObjectKey(found.ImageUrl);
            var url = await _storage.UploadAsync(
                image.OpenReadStream(),
                image.FileName,
                image.ContentType,
                UploadPrefixes.Section(key)
            );
            request.ImageUrl = url;
            if (!string.IsNullOrWhiteSpace(oldKey))
            {
                await _storage.DeleteAsync(oldKey!);
            }
        }
        await _service.UpdateAsync(key, request);
        return NoContent();
    }

    [HttpPut("why_us")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateWhyUs(
        [FromForm] WhyUsUpsertDto dto,
        [FromForm] List<IFormFile>? imageFiles,
        [FromForm] List<int>? imageIndexes
    )
    {
        var key = "why_us";
        var found = await _service.GetByKeyAsync(key);
        if (found == null)
            return NotFound();
        List<string> oldImages = new();
        try
        {
            var md = !string.IsNullOrWhiteSpace(found.MoreData)
                ? JsonSerializer.Deserialize<JsonElement>(found.MoreData!)
                : default;
            if (md.ValueKind != JsonValueKind.Undefined)
            {
                JsonElement imgs;
                if (
                    (md.TryGetProperty("images", out imgs) || md.TryGetProperty("Images", out imgs))
                    && imgs.ValueKind == JsonValueKind.Array
                )
                {
                    foreach (var el in imgs.EnumerateArray())
                        oldImages.Add(el.GetString() ?? "");
                }
            }
        }
        catch { }

        var images =
            (dto.Images != null && dto.Images.Count > 0)
                ? new List<string>(dto.Images)
                : new List<string>(oldImages);
        while (images.Count < 4)
            images.Add("");
        while (oldImages.Count < 4)
            oldImages.Add("");

        for (int i = 0; i < images.Count && i < oldImages.Count; i++)
        {
            var v = images[i];
            if (
                !string.IsNullOrWhiteSpace(v)
                && v.StartsWith("blob:", StringComparison.OrdinalIgnoreCase)
            )
            {
                images[i] = oldImages[i];
            }
        }

        if (imageFiles != null && imageFiles.Count > 0)
        {
            for (int k = 0; k < imageFiles.Count; k++)
            {
                var f = imageFiles[k];
                var i = (imageIndexes != null && k < imageIndexes.Count) ? imageIndexes[k] : k;
                if (i < 0 || i > 3)
                    continue;
                if (f != null && f.Length > 0)
                {
                    var newUrl = await _storage.UploadAsync(
                        f.OpenReadStream(),
                        f.FileName,
                        f.ContentType,
                        UploadPrefixes.Section($"{key}/{i}")
                    );
                    var oldUrl = oldImages[i];
                    images[i] = newUrl;
                    var oldKey = S3KeyHelper.TryExtractObjectKey(oldUrl);
                    if (!string.IsNullOrWhiteSpace(oldKey))
                    {
                        await _storage.DeleteAsync(oldKey!);
                    }
                }
            }
        }

        while (dto.Bullets.Count < 4)
            dto.Bullets.Add("");
        while (images.Count < 4)
            images.Add("");

        var payload = new
        {
            title = dto.Title,
            subtitle = dto.Subtitle,
            bullets = dto.Bullets,
            images = images,
        };
        var json = JsonSerializer.Serialize(
            payload,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        var up = new SectionContentUpsertDto
        {
            SectionKey = key,
            Content = dto.Subtitle,
            ImageUrl = null,
            MoreData = json,
        };
        await _service.UpdateAsync(key, up);
        return NoContent();
    }

    [HttpPut("about")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAbout([FromForm] AboutUpsertDto dto, IFormFile? image)
    {
        var key = "about";
        var found = await _service.GetByKeyAsync(key);
        if (found == null)
            return NotFound();
        string? imageUrl = dto.Image;
        if (image != null && image.Length > 0)
        {
            var oldKey = S3KeyHelper.TryExtractObjectKey(found.ImageUrl);
            imageUrl = await _storage.UploadAsync(
                image.OpenReadStream(),
                image.FileName,
                image.ContentType,
                UploadPrefixes.Section(key)
            );
            if (!string.IsNullOrWhiteSpace(oldKey))
                await _storage.DeleteAsync(oldKey!);
        }
        var json = JsonSerializer.Serialize(
            new
            {
                title = dto.Title,
                content = dto.Content,
                image = imageUrl,
            },
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
        );
        var up = new SectionContentUpsertDto
        {
            SectionKey = key,
            Content = dto.Content,
            ImageUrl = imageUrl,
            MoreData = json,
        };
        await _service.UpdateAsync(key, up);
        return NoContent();
    }

    [HttpDelete("{key}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] string key)
    {
        var found = await _service.GetByKeyAsync(key);
        if (found == null)
            return NotFound();
        await _service.DeleteAsync(key);
        return NoContent();
    }
}
