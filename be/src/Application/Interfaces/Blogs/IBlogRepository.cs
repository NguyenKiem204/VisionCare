using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Blogs;

public interface IBlogRepository
{
    Task<IReadOnlyList<Blog>> GetAllAsync(CancellationToken ct = default);
    Task<Blog?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Blog?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<IReadOnlyList<Blog>> GetByAuthorIdAsync(int authorId, CancellationToken ct = default);
    Task<IReadOnlyList<Blog>> GetPublishedAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Blog>> SearchAsync(
        string? keyword,
        int? authorId,
        string? status,
        DateTime? publishedFrom,
        DateTime? publishedTo,
        CancellationToken ct = default
    );
    Task<(IReadOnlyList<Blog> items, int totalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? keyword,
        int? authorId,
        string? status,
        DateTime? publishedFrom,
        DateTime? publishedTo,
        CancellationToken ct = default
    );
    Task CreateAsync(Blog blog, CancellationToken ct = default);
    Task UpdateAsync(Blog blog, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<bool> SlugExistsAsync(string slug, int? excludeBlogId = null, CancellationToken ct = default);
    Task IncrementViewCountAsync(int id, CancellationToken ct = default);
}

