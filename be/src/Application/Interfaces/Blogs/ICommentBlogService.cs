using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.DTOs.BlogDto;

namespace VisionCare.Application.Interfaces.Blogs;

public interface ICommentBlogService
{
    Task<IReadOnlyList<CommentBlogDto>> GetByBlogIdAsync(int blogId, CancellationToken ct = default);
    Task<CommentBlogDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CommentBlogDto> CreateAsync(CreateCommentRequest request, int authorId, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateCommentRequest request, int currentUserId, string currentUserRole, CancellationToken ct = default);
    Task DeleteAsync(int id, int currentUserId, string currentUserRole, CancellationToken ct = default);
}

