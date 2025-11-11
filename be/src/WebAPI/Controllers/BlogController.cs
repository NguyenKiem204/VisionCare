using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.BlogDto;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Blogs;
using VisionCare.WebAPI.Responses;
using VisionCare.WebAPI.Utils;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;
    private readonly IS3StorageService _storage;

    public BlogController(IBlogService blogService, IS3StorageService storage)
    {
        _blogService = blogService;
        _storage = storage;
    }

    private int? GetCurrentAccountId()
    {
        var idClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("account_id")?.Value
            ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out var accountId))
        {
            return null;
        }
        return accountId;
    }

    private string? GetCurrentUserRole()
    {
        return User.FindFirst(ClaimTypes.Role)?.Value ?? User.FindFirst("role")?.Value;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublished([FromQuery] BlogSearchRequest request)
    {
        try
        {
            request.Status = "Published";
            var result = await _blogService.SearchAsync(request);
            return Ok(
                PagedResponse<BlogDto>.Ok(
                    result.items,
                    result.totalCount,
                    request.Page,
                    request.PageSize
                )
            );
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail(
                    "An error occurred while loading blogs. Please try again later."
                )
            );
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound(ApiResponse<BlogDto>.Fail("Blog not found"));
            }
            return Ok(ApiResponse<BlogDto>.Ok(blog));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail(
                    "An error occurred while loading the blog. Please try again later."
                )
            );
        }
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug([FromRoute] string slug)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return BadRequest(ApiResponse<object>.Fail("Slug is required"));
            }

            var blog = await _blogService.GetBySlugAsync(slug);
            if (blog == null)
            {
                return NotFound(ApiResponse<BlogDto>.Fail("Blog not found"));
            }
            return Ok(ApiResponse<BlogDto>.Ok(blog));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail(
                    "An error occurred while loading the blog. Please try again later."
                )
            );
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyBlogs([FromQuery] BlogSearchRequest request)
    {
        var accountId = GetCurrentAccountId();
        if (!accountId.HasValue)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
        }

        request.AuthorId = accountId.Value;
        var result = await _blogService.SearchAsync(request);
        return Ok(
            PagedResponse<BlogDto>.Ok(
                result.items,
                result.totalCount,
                request.Page,
                request.PageSize
            )
        );
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] BlogSearchRequest request)
    {
        var result = await _blogService.SearchAsync(request);
        return Ok(
            PagedResponse<BlogDto>.Ok(
                result.items,
                result.totalCount,
                request.Page,
                request.PageSize
            )
        );
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Doctor,Staff")]
    public async Task<IActionResult> Create(
        [FromForm] CreateBlogRequest request,
        IFormFile? featuredImage
    )
    {
        var accountId = GetCurrentAccountId();
        if (!accountId.HasValue)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
        }

        var role = GetCurrentUserRole();
        if (role == "Staff")
        {
            request.Status = "Draft";
        }

        try
        {
            var blog = await _blogService.CreateAsync(request, accountId.Value);

            if (featuredImage != null && featuredImage.Length > 0)
            {
                var url = await _storage.UploadAsync(
                    featuredImage.OpenReadStream(),
                    featuredImage.FileName,
                    featuredImage.ContentType,
                    UploadPrefixes.Blog(blog.BlogId)
                );

                var updateRequest = new UpdateBlogRequest
                {
                    Title = blog.Title,
                    Slug = blog.Slug,
                    Content = blog.Content,
                    Excerpt = blog.Excerpt,
                    FeaturedImage = url,
                    Status = blog.Status,
                };
                await _blogService.UpdateAsync(blog.BlogId, updateRequest, accountId.Value, role ?? string.Empty);
                // reflect updated image in response without re-fetching
                blog.FeaturedImage = url;
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = blog.BlogId },
                ApiResponse<BlogDto>.Ok(blog)
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail(
                    "An error occurred while creating the blog. Please try again later."
                )
            );
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Doctor,Staff")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromForm] UpdateBlogRequest request,
        IFormFile? featuredImage
    )
    {
        var accountId = GetCurrentAccountId();
        if (!accountId.HasValue)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
        }

        var role = GetCurrentUserRole() ?? "";
        if (role == "Staff")
        {
            if (request.Status == "Published")
            {
                return BadRequest(ApiResponse<object>.Fail("Staff cannot publish blogs"));
            }
        }

        try
        {
            var currentBlog = await _blogService.GetByIdAsync(id);
            if (currentBlog == null)
            {
                return NotFound(ApiResponse<object>.Fail("Blog not found"));
            }

            if (featuredImage != null && featuredImage.Length > 0)
            {
                var oldKey = S3KeyHelper.TryExtractObjectKey(currentBlog.FeaturedImage);
                if (!string.IsNullOrWhiteSpace(oldKey))
                {
                    await _storage.DeleteAsync(oldKey!);
                }

                var url = await _storage.UploadAsync(
                    featuredImage.OpenReadStream(),
                    featuredImage.FileName,
                    featuredImage.ContentType,
                    UploadPrefixes.Blog(id)
                );
                request.FeaturedImage = url;
            }

            await _blogService.UpdateAsync(id, request, accountId.Value, role);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message ?? "Resource not found"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                403,
                ApiResponse<object>.Fail(
                    ex.Message ?? "You do not have permission to perform this action"
                )
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail("An error occurred. Please try again later.")
            );
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var accountId = GetCurrentAccountId();
        if (!accountId.HasValue)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
        }

        var role = GetCurrentUserRole() ?? "";

        try
        {
            await _blogService.DeleteAsync(id, accountId.Value, role);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message ?? "Resource not found"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                403,
                ApiResponse<object>.Fail(
                    ex.Message ?? "You do not have permission to perform this action"
                )
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail("An error occurred. Please try again later.")
            );
        }
    }

    [HttpPost("{id:int}/publish")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Publish([FromRoute] int id)
    {
        var accountId = GetCurrentAccountId();
        if (!accountId.HasValue)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
        }

        var role = GetCurrentUserRole() ?? "";

        try
        {
            await _blogService.PublishAsync(id, accountId.Value, role);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message ?? "Resource not found"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                403,
                ApiResponse<object>.Fail(
                    ex.Message ?? "You do not have permission to perform this action"
                )
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail("An error occurred. Please try again later.")
            );
        }
    }

    [HttpPost("{id:int}/unpublish")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Unpublish([FromRoute] int id)
    {
        var accountId = GetCurrentAccountId();
        if (!accountId.HasValue)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
        }

        var role = GetCurrentUserRole() ?? "";

        try
        {
            await _blogService.UnpublishAsync(id, accountId.Value, role);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message ?? "Resource not found"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                403,
                ApiResponse<object>.Fail(
                    ex.Message ?? "You do not have permission to perform this action"
                )
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail("An error occurred. Please try again later.")
            );
        }
    }

    [HttpPost("{id:int}/view")]
    public async Task<IActionResult> IncrementView([FromRoute] int id)
    {
        try
        {
            await _blogService.IncrementViewCountAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message ?? "Blog not found"));
        }
        catch (Exception)
        {
            return NoContent();
        }
    }

    [HttpGet("check-slug")]
    public async Task<IActionResult> CheckSlug(
        [FromQuery] string slug,
        [FromQuery] int? excludeBlogId = null
    )
    {
        var exists = await _blogService.SlugExistsAsync(slug, excludeBlogId);
        return Ok(new { exists, slug });
    }
}
