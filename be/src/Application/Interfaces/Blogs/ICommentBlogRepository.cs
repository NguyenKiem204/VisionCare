using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Blogs;

public interface ICommentBlogRepository
{
    Task<IReadOnlyList<CommentBlog>> GetByBlogIdAsync(int blogId, CancellationToken ct = default);
    Task<IReadOnlyList<(CommentBlog comment, string? authorName, string? authorAvatar)>> GetByBlogIdWithAuthorAsync(int blogId, CancellationToken ct = default);
    Task<CommentBlog?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<(CommentBlog comment, string? authorName, string? authorAvatar)?> GetByIdWithAuthorAsync(int id, CancellationToken ct = default);
    Task CreateAsync(CommentBlog comment, CancellationToken ct = default);
    Task UpdateAsync(CommentBlog comment, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<int> GetCommentCountByBlogIdAsync(int blogId, CancellationToken ct = default);
}

