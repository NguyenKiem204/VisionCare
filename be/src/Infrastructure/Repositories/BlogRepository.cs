using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.Interfaces.Blogs;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly VisionCareDbContext _db;

    public BlogRepository(VisionCareDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Blog>> GetAllAsync(CancellationToken ct = default)
    {
        var blogs = await _db.Blogs
            .AsNoTracking()
            .Include(b => b.Author)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(ct);
        return blogs.Select(BlogMapper.ToDomain).ToList();
    }

    public async Task<Blog?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var blog = await _db.Blogs
            .AsNoTracking()
            .Include(b => b.Author)
            .ThenInclude(a => a.Customer)
            .Include(b => b.Author)
            .ThenInclude(a => a.Doctor)
            .Include(b => b.Author)
            .ThenInclude(a => a.Staff)
            .FirstOrDefaultAsync(b => b.BlogId == id, ct);
        return blog == null ? null : BlogMapper.ToDomain(blog);
    }

    public async Task<Blog?> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var blog = await _db.Blogs
            .AsNoTracking()
            .Include(b => b.Author)
            .ThenInclude(a => a.Customer)
            .Include(b => b.Author)
            .ThenInclude(a => a.Doctor)
            .Include(b => b.Author)
            .ThenInclude(a => a.Staff)
            .FirstOrDefaultAsync(b => b.Slug == slug, ct);
        return blog == null ? null : BlogMapper.ToDomain(blog);
    }

    public async Task<IReadOnlyList<Blog>> GetByAuthorIdAsync(int authorId, CancellationToken ct = default)
    {
        var blogs = await _db.Blogs
            .AsNoTracking()
            .Include(b => b.Author)
            .Where(b => b.AuthorId == authorId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(ct);
        return blogs.Select(BlogMapper.ToDomain).ToList();
    }

    public async Task<IReadOnlyList<Blog>> GetPublishedAsync(CancellationToken ct = default)
    {
        var blogs = await _db.Blogs
            .AsNoTracking()
            .Include(b => b.Author)
            .Where(b => b.Status == "Published" && b.PublishedAt != null)
            .OrderByDescending(b => b.PublishedAt)
            .ToListAsync(ct);
        return blogs.Select(BlogMapper.ToDomain).ToList();
    }

    public async Task<IReadOnlyList<Blog>> SearchAsync(
        string? keyword,
        int? authorId,
        string? status,
        DateTime? publishedFrom,
        DateTime? publishedTo,
        CancellationToken ct = default)
    {
        var query = _db.Blogs
            .AsNoTracking()
            .Include(b => b.Author)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(b =>
                b.Title.Contains(keyword) ||
                b.Content.Contains(keyword) ||
                b.Excerpt != null && b.Excerpt.Contains(keyword));
        }

        if (authorId.HasValue)
        {
            query = query.Where(b => b.AuthorId == authorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(b => b.Status == status);
        }

        if (publishedFrom.HasValue)
        {
            query = query.Where(b => b.PublishedAt >= publishedFrom.Value);
        }

        if (publishedTo.HasValue)
        {
            query = query.Where(b => b.PublishedAt <= publishedTo.Value);
        }

        var blogs = await query
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(ct);
        return blogs.Select(BlogMapper.ToDomain).ToList();
    }

    public async Task<(IReadOnlyList<Blog> items, int totalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? keyword,
        int? authorId,
        string? status,
        DateTime? publishedFrom,
        DateTime? publishedTo,
        CancellationToken ct = default)
    {
        var query = _db.Blogs
            .AsNoTracking()
            .Include(b => b.Author)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(b =>
                b.Title.Contains(keyword) ||
                b.Content.Contains(keyword) ||
                b.Excerpt != null && b.Excerpt.Contains(keyword));
        }

        if (authorId.HasValue)
        {
            query = query.Where(b => b.AuthorId == authorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(b => b.Status == status);
        }

        if (publishedFrom.HasValue)
        {
            query = query.Where(b => b.PublishedAt >= publishedFrom.Value);
        }

        if (publishedTo.HasValue)
        {
            query = query.Where(b => b.PublishedAt <= publishedTo.Value);
        }

        var totalCount = await query.CountAsync(ct);

        var blogs = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (blogs.Select(BlogMapper.ToDomain).ToList(), totalCount);
    }

    public async Task CreateAsync(Blog blog, CancellationToken ct = default)
    {
        var model = BlogMapper.ToInfrastructure(blog);
        model.CreatedAt = DateTime.Now;
        model.UpdatedAt = DateTime.Now;
        _db.Blogs.Add(model);
        await _db.SaveChangesAsync(ct);
        blog.BlogId = model.BlogId;
    }

    public async Task UpdateAsync(Blog blog, CancellationToken ct = default)
    {
        var model = await _db.Blogs.FirstOrDefaultAsync(b => b.BlogId == blog.BlogId, ct);
        if (model == null) return;

        model.Title = blog.Title;
        model.Slug = blog.Slug;
        model.Content = blog.Content;
        model.Excerpt = blog.Excerpt;
        model.FeaturedImage = blog.FeaturedImage;
        model.Status = blog.Status;
        model.PublishedAt = blog.PublishedAt?.ToLocalTime();
        model.UpdatedAt = DateTime.Now;

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var model = await _db.Blogs.FirstOrDefaultAsync(b => b.BlogId == id, ct);
        if (model == null) return;
        _db.Blogs.Remove(model);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await _db.Blogs.AnyAsync(b => b.BlogId == id, ct);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeBlogId = null, CancellationToken ct = default)
    {
        var query = _db.Blogs.Where(b => b.Slug == slug);
        if (excludeBlogId.HasValue)
        {
            query = query.Where(b => b.BlogId != excludeBlogId.Value);
        }
        return await query.AnyAsync(ct);
    }

    public async Task IncrementViewCountAsync(int id, CancellationToken ct = default)
    {
        var model = await _db.Blogs.FirstOrDefaultAsync(b => b.BlogId == id, ct);
        if (model == null) return;
        model.ViewCount = (model.ViewCount ?? 0) + 1;
        await _db.SaveChangesAsync(ct);
    }
}

