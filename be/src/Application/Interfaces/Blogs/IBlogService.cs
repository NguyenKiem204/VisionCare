using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.DTOs.BlogDto;

namespace VisionCare.Application.Interfaces.Blogs;

public interface IBlogService
{
    Task<BlogDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<BlogDto?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<IReadOnlyList<BlogDto>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<BlogDto>> GetPublishedAsync(CancellationToken ct = default);
    Task<IReadOnlyList<BlogDto>> GetByAuthorIdAsync(int authorId, CancellationToken ct = default);
    Task<(IReadOnlyList<BlogDto> items, int totalCount)> SearchAsync(BlogSearchRequest request, CancellationToken ct = default);
    Task<BlogDto> CreateAsync(CreateBlogRequest request, int authorId, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateBlogRequest request, int currentUserId, string currentUserRole, CancellationToken ct = default);
    Task DeleteAsync(int id, int currentUserId, string currentUserRole, CancellationToken ct = default);
    Task PublishAsync(int id, int currentUserId, string currentUserRole, CancellationToken ct = default);
    Task UnpublishAsync(int id, int currentUserId, string currentUserRole, CancellationToken ct = default);
    Task IncrementViewCountAsync(int id, CancellationToken ct = default);
    Task<bool> SlugExistsAsync(string slug, int? excludeBlogId = null, CancellationToken ct = default);
}

