using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VisionCare.Application.DTOs.BlogDto;
using VisionCare.Application.Interfaces.Blogs;
using VisionCare.WebAPI.Hubs;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/blog/{blogId}/comments")]
public class CommentBlogController : ControllerBase
{
    private readonly ICommentBlogService _commentService;
    private readonly IHubContext<CommentHub> _hubContext;
    private readonly ILogger<CommentBlogController> _logger;

    public CommentBlogController(
        ICommentBlogService commentService,
        IHubContext<CommentHub> hubContext,
        ILogger<CommentBlogController> logger
    )
    {
        _commentService = commentService;
        _hubContext = hubContext;
        _logger = logger;
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
    public async Task<IActionResult> GetComments([FromRoute] int blogId)
    {
        try
        {
            var comments = await _commentService.GetByBlogIdAsync(blogId);
            return Ok(ApiResponse<IReadOnlyList<CommentBlogDto>>.Ok(comments));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message ?? "Blog not found"));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail(
                    "An error occurred while loading comments. Please try again later."
                )
            );
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateComment(
        [FromRoute] int blogId,
        [FromBody] CreateCommentRequest request
    )
    {
        var accountId = GetCurrentAccountId();
        if (!accountId.HasValue)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
        }
        if (blogId <= 0)
        {
            return BadRequest(ApiResponse<object>.Fail("Invalid blog ID"));
        }

        request.BlogId = blogId;

        try
        {
            var comment = await _commentService.CreateAsync(request, accountId.Value);

            try
            {
                await _hubContext.Clients.Group($"blog:{blogId}").SendAsync("NewComment", comment);
            }
            catch { }

            return CreatedAtAction(
                nameof(GetComment),
                new { blogId, id = comment.CommentId },
                ApiResponse<CommentBlogDto>.Ok(comment)
            );
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.Fail(ex.Message ?? "Blog not found"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating comment for blog {BlogId}", blogId);
            return StatusCode(
                500,
                ApiResponse<object>.Fail(
                    "An error occurred while creating the comment. Please try again later."
                )
            );
        }
    }

    [HttpGet("{id:int}", Name = "GetComment")]
    public async Task<IActionResult> GetComment([FromRoute] int blogId, [FromRoute] int id)
    {
        try
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null || comment.BlogId != blogId)
            {
                return NotFound(ApiResponse<CommentBlogDto>.Fail("Comment not found"));
            }
            return Ok(ApiResponse<CommentBlogDto>.Ok(comment));
        }
        catch (Exception)
        {
            return StatusCode(
                500,
                ApiResponse<object>.Fail(
                    "An error occurred while loading the comment. Please try again later."
                )
            );
        }
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] int blogId,
        [FromRoute] int id,
        [FromBody] UpdateCommentRequest request
    )
    {
        var accountId = GetCurrentAccountId();
        if (!accountId.HasValue)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
        }

        var role = GetCurrentUserRole() ?? "";

        try
        {
            await _commentService.UpdateAsync(id, request, accountId.Value, role);
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
    [Authorize]
    public async Task<IActionResult> DeleteComment([FromRoute] int blogId, [FromRoute] int id)
    {
        var accountId = GetCurrentAccountId();
        if (!accountId.HasValue)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
        }

        var role = GetCurrentUserRole() ?? "";

        try
        {
            await _commentService.DeleteAsync(id, accountId.Value, role);
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
}
