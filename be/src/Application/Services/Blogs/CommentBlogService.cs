using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.DTOs.BlogDto;
using VisionCare.Application.Interfaces.Blogs;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Blogs;

public class CommentBlogService : ICommentBlogService
{
    private readonly ICommentBlogRepository _commentRepository;
    private readonly IBlogRepository _blogRepository;

    public CommentBlogService(ICommentBlogRepository commentRepository, IBlogRepository blogRepository)
    {
        _commentRepository = commentRepository;
        _blogRepository = blogRepository;
    }

    public async Task<IReadOnlyList<CommentBlogDto>> GetByBlogIdAsync(int blogId, CancellationToken ct = default)
    {
        // Verify blog exists
        var blogExists = await _blogRepository.ExistsAsync(blogId, ct);
        if (!blogExists)
            throw new KeyNotFoundException($"Blog with ID {blogId} not found");

        var comments = await _commentRepository.GetByBlogIdWithAuthorAsync(blogId, ct);
        
        // Build nested structure with author info
        var commentDtos = comments.Select(c => ToDtoWithAuthor(c.comment, c.authorName, c.authorAvatar)).ToList();
        var parentComments = commentDtos.Where(c => c.ParentCommentId == null).ToList();
        
        foreach (var parent in parentComments)
        {
            parent.Replies = commentDtos
                .Where(c => c.ParentCommentId == parent.CommentId)
                .OrderBy(c => c.CreatedAt)
                .ToList();
        }

        return parentComments.OrderBy(c => c.CreatedAt).ToList();
    }

    public async Task<CommentBlogDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var result = await _commentRepository.GetByIdWithAuthorAsync(id, ct);
        if (result == null) return null;
        
        var (comment, authorName, authorAvatar) = result.Value;
        return ToDtoWithAuthor(comment, authorName, authorAvatar);
    }

    public async Task<CommentBlogDto> CreateAsync(
        CreateCommentRequest request,
        int authorId,
        CancellationToken ct = default)
    {
        // Verify blog exists
        var blogExists = await _blogRepository.ExistsAsync(request.BlogId, ct);
        if (!blogExists)
            throw new KeyNotFoundException($"Blog with ID {request.BlogId} not found");

        // Verify parent comment exists if provided
        if (request.ParentCommentId.HasValue)
        {
            var parentExists = await _commentRepository.ExistsAsync(request.ParentCommentId.Value, ct);
            if (!parentExists)
                throw new KeyNotFoundException($"Parent comment with ID {request.ParentCommentId.Value} not found");
        }

        var comment = new CommentBlog
        {
            BlogId = request.BlogId,
            AuthorId = authorId,
            ParentCommentId = request.ParentCommentId,
            CommentText = request.CommentText,
            Status = "Active",
        };

        await _commentRepository.CreateAsync(comment, ct);

        // Reload with author info
        var result = await _commentRepository.GetByIdWithAuthorAsync(comment.CommentId, ct);
        if (result == null) return ToDto(comment);
        
        var (createdComment, authorName, authorAvatar) = result.Value;
        return ToDtoWithAuthor(createdComment, authorName, authorAvatar);
    }

    public async Task UpdateAsync(
        int id,
        UpdateCommentRequest request,
        int currentUserId,
        string currentUserRole,
        CancellationToken ct = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, ct);
        if (comment == null)
            throw new KeyNotFoundException($"Comment with ID {id} not found");

        // Permission check: Admin can update all, others can only update their own
        if (currentUserRole != "Admin" && comment.AuthorId != currentUserId)
            throw new UnauthorizedAccessException("You can only update your own comments");

        comment.CommentText = request.CommentText;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            comment.Status = request.Status;
        }

        await _commentRepository.UpdateAsync(comment, ct);
    }

    public async Task DeleteAsync(int id, int currentUserId, string currentUserRole, CancellationToken ct = default)
    {
        var comment = await _commentRepository.GetByIdAsync(id, ct);
        if (comment == null)
            throw new KeyNotFoundException($"Comment with ID {id} not found");

        // Permission check: Admin can delete all, others can only delete their own
        if (currentUserRole != "Admin" && comment.AuthorId != currentUserId)
            throw new UnauthorizedAccessException("You can only delete your own comments");

        await _commentRepository.DeleteAsync(id, ct);
    }

    private static CommentBlogDto ToDto(CommentBlog comment)
    {
        return new CommentBlogDto
        {
            CommentId = comment.CommentId,
            BlogId = comment.BlogId,
            AuthorId = comment.AuthorId,
            AuthorName = null,
            AuthorAvatar = null,
            ParentCommentId = comment.ParentCommentId,
            CommentText = comment.CommentText,
            Status = comment.Status,
            CreatedAt = comment.CreatedAt,
            Replies = new List<CommentBlogDto>(),
        };
    }

    private static CommentBlogDto ToDtoWithAuthor(CommentBlog comment, string? authorName = null, string? authorAvatar = null)
    {
        return new CommentBlogDto
        {
            CommentId = comment.CommentId,
            BlogId = comment.BlogId,
            AuthorId = comment.AuthorId,
            AuthorName = authorName,
            AuthorAvatar = authorAvatar,
            ParentCommentId = comment.ParentCommentId,
            CommentText = comment.CommentText,
            Status = comment.Status,
            CreatedAt = comment.CreatedAt,
            Replies = new List<CommentBlogDto>(),
        };
    }
}

