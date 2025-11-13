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

public class CommentBlogRepository : ICommentBlogRepository
{
    private readonly VisionCareDbContext _db;

    public CommentBlogRepository(VisionCareDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CommentBlog>> GetByBlogIdAsync(int blogId, CancellationToken ct = default)
    {
        var comments = await _db.Commentblogs
            .AsNoTracking()
            .Include(c => c.Author!)
                .ThenInclude(a => a.Customer)
            .Include(c => c.Author!)
                .ThenInclude(a => a.Doctor)
            .Include(c => c.Author!)
                .ThenInclude(a => a.Staff)
            .Where(c => c.BlogId == blogId && c.Status == "Active")
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);
        return comments.Select(CommentBlogMapper.ToDomain).ToList();
    }

    public async Task<IReadOnlyList<(CommentBlog comment, string? authorName, string? authorAvatar)>> GetByBlogIdWithAuthorAsync(int blogId, CancellationToken ct = default)
    {
        var comments = await _db.Commentblogs
            .AsNoTracking()
            .Include(c => c.Author!)
                .ThenInclude(a => a.Customer)
            .Include(c => c.Author!)
                .ThenInclude(a => a.Doctor)
            .Include(c => c.Author!)
                .ThenInclude(a => a.Staff)
            .Where(c => c.BlogId == blogId && c.Status == "Active")
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);
        
        return comments.Select(c =>
        {
            var (name, avatar) = ResolveAuthorInfo(c);
            return (CommentBlogMapper.ToDomain(c), name, avatar);
        }).ToList();
    }

    // Helper method to resolve author name and avatar from Account model
    public static (string? name, string? avatar) ResolveAuthorInfo(VisionCare.Infrastructure.Models.Commentblog? model)
    {
        if (model?.Author == null) return (null, null);

        var account = model.Author;
        string? name = null;
        string? avatar = null;

        if (account.Customer != null)
        {
            name = account.Customer.FullName;
            avatar = account.Customer.Avatar;
        }
        else if (account.Doctor != null)
        {
            name = account.Doctor.FullName;
            avatar = account.Doctor.Avatar;
        }
        else if (account.Staff != null)
        {
            name = account.Staff.FullName;
            avatar = account.Staff.Avatar;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            name = account.Username;
        }

        return (name, avatar);
    }

    public async Task<CommentBlog?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var comment = await _db.Commentblogs
            .AsNoTracking()
            .Include(c => c.Author!)
                .ThenInclude(a => a.Customer)
            .Include(c => c.Author!)
                .ThenInclude(a => a.Doctor)
            .Include(c => c.Author!)
                .ThenInclude(a => a.Staff)
            .FirstOrDefaultAsync(c => c.CommentId == id, ct);
        return comment == null ? null : CommentBlogMapper.ToDomain(comment);
    }

    public async Task<(CommentBlog comment, string? authorName, string? authorAvatar)?> GetByIdWithAuthorAsync(int id, CancellationToken ct = default)
    {
        var comment = await _db.Commentblogs
            .AsNoTracking()
            .Include(c => c.Author!)
                .ThenInclude(a => a.Customer)
            .Include(c => c.Author!)
                .ThenInclude(a => a.Doctor)
            .Include(c => c.Author!)
                .ThenInclude(a => a.Staff)
            .FirstOrDefaultAsync(c => c.CommentId == id, ct);
        
        if (comment == null) return null;
        
        var (name, avatar) = ResolveAuthorInfo(comment);
        return (CommentBlogMapper.ToDomain(comment), name, avatar);
    }

    public async Task CreateAsync(CommentBlog comment, CancellationToken ct = default)
    {
        var model = CommentBlogMapper.ToInfrastructure(comment);
        model.CreatedAt = DateTime.Now;
        model.Status = model.Status ?? "Active";
        _db.Commentblogs.Add(model);
        await _db.SaveChangesAsync(ct);
        comment.CommentId = model.CommentId;
    }

    public async Task UpdateAsync(CommentBlog comment, CancellationToken ct = default)
    {
        var model = await _db.Commentblogs.FirstOrDefaultAsync(c => c.CommentId == comment.CommentId, ct);
        if (model == null) return;

        model.CommentText = comment.CommentText;
        model.Status = comment.Status;

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var model = await _db.Commentblogs.FirstOrDefaultAsync(c => c.CommentId == id, ct);
        if (model == null) return;
        _db.Commentblogs.Remove(model);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
    {
        return await _db.Commentblogs.AnyAsync(c => c.CommentId == id, ct);
    }

    public async Task<int> GetCommentCountByBlogIdAsync(int blogId, CancellationToken ct = default)
    {
        return await _db.Commentblogs
            .CountAsync(c => c.BlogId == blogId && c.Status == "Active", ct);
    }
}

