using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.DTOs.BlogDto;
using VisionCare.Application.Interfaces.Blogs;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Blogs;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly ICommentBlogRepository _commentRepository;

    public BlogService(IBlogRepository blogRepository, ICommentBlogRepository commentRepository)
    {
        _blogRepository = blogRepository;
        _commentRepository = commentRepository;
    }

    public async Task<BlogDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var blog = await _blogRepository.GetByIdAsync(id, ct);
        if (blog == null) return null;

        var commentCount = await _commentRepository.GetCommentCountByBlogIdAsync(id, ct);
        return ToDto(blog, commentCount);
    }

    public async Task<BlogDto?> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var blog = await _blogRepository.GetBySlugAsync(slug, ct);
        if (blog == null) return null;

        var commentCount = await _commentRepository.GetCommentCountByBlogIdAsync(blog.BlogId, ct);
        return ToDto(blog, commentCount);
    }

    public async Task<IReadOnlyList<BlogDto>> GetAllAsync(CancellationToken ct = default)
    {
        var blogs = await _blogRepository.GetAllAsync(ct);
        var dtos = new List<BlogDto>();

        foreach (var blog in blogs)
        {
            var commentCount = await _commentRepository.GetCommentCountByBlogIdAsync(blog.BlogId, ct);
            dtos.Add(ToDto(blog, commentCount));
        }

        return dtos;
    }

    public async Task<IReadOnlyList<BlogDto>> GetPublishedAsync(CancellationToken ct = default)
    {
        var blogs = await _blogRepository.GetPublishedAsync(ct);
        var dtos = new List<BlogDto>();

        foreach (var blog in blogs)
        {
            var commentCount = await _commentRepository.GetCommentCountByBlogIdAsync(blog.BlogId, ct);
            dtos.Add(ToDto(blog, commentCount));
        }

        return dtos;
    }

    public async Task<IReadOnlyList<BlogDto>> GetByAuthorIdAsync(int authorId, CancellationToken ct = default)
    {
        var blogs = await _blogRepository.GetByAuthorIdAsync(authorId, ct);
        var dtos = new List<BlogDto>();

        foreach (var blog in blogs)
        {
            var commentCount = await _commentRepository.GetCommentCountByBlogIdAsync(blog.BlogId, ct);
            dtos.Add(ToDto(blog, commentCount));
        }

        return dtos;
    }

    public async Task<(IReadOnlyList<BlogDto> items, int totalCount)> SearchAsync(
        BlogSearchRequest request,
        CancellationToken ct = default)
    {
        var result = await _blogRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Keyword,
            request.AuthorId,
            request.Status,
            request.PublishedFrom,
            request.PublishedTo,
            ct
        );

        var dtos = new List<BlogDto>();
        foreach (var blog in result.items)
        {
            var commentCount = await _commentRepository.GetCommentCountByBlogIdAsync(blog.BlogId, ct);
            dtos.Add(ToDto(blog, commentCount));
        }

        return (dtos, result.totalCount);
    }

    public async Task<BlogDto> CreateAsync(CreateBlogRequest request, int authorId, CancellationToken ct = default)
    {
        // Generate slug if not provided
        var slug = string.IsNullOrWhiteSpace(request.Slug)
            ? await GenerateUniqueSlugAsync(request.Title, null, ct)
            : await GenerateUniqueSlugAsync(request.Slug, null, ct);

        var blog = new Blog
        {
            Title = request.Title,
            Slug = slug,
            Content = request.Content,
            Excerpt = request.Excerpt,
            FeaturedImage = request.FeaturedImage,
            AuthorId = authorId,
            Status = request.Status ?? "Draft",
            PublishedAt = request.Status == "Published" ? DateTime.Now : null,
            ViewCount = 0,
        };

        await _blogRepository.CreateAsync(blog, ct);

        return ToDto(blog, 0);
    }

    public async Task UpdateAsync(
        int id,
        UpdateBlogRequest request,
        int currentUserId,
        string currentUserRole,
        CancellationToken ct = default)
    {
        var blog = await _blogRepository.GetByIdAsync(id, ct);
        if (blog == null)
            throw new KeyNotFoundException($"Blog with ID {id} not found");

        // Permission check: Admin can update all, others can only update their own
        if (currentUserRole != "Admin" && blog.AuthorId != currentUserId)
            throw new UnauthorizedAccessException("You can only update your own blogs");

        // Generate slug if changed
        var slug = string.IsNullOrWhiteSpace(request.Slug)
            ? await GenerateUniqueSlugAsync(request.Title, id, ct)
            : await GenerateUniqueSlugAsync(request.Slug, id, ct);

        blog.Title = request.Title;
        blog.Slug = slug;
        blog.Content = request.Content;
        blog.Excerpt = request.Excerpt;
        blog.FeaturedImage = request.FeaturedImage;
        blog.Status = request.Status ?? blog.Status;

        // Update PublishedAt if status changed to Published
        if (request.Status == "Published" && blog.PublishedAt == null)
        {
            blog.PublishedAt = DateTime.Now;
        }
        else if (request.Status != "Published" && blog.PublishedAt != null)
        {
            blog.PublishedAt = null;
        }

        await _blogRepository.UpdateAsync(blog, ct);
    }

    public async Task DeleteAsync(int id, int currentUserId, string currentUserRole, CancellationToken ct = default)
    {
        var blog = await _blogRepository.GetByIdAsync(id, ct);
        if (blog == null)
            throw new KeyNotFoundException($"Blog with ID {id} not found");

        // Permission check: Admin can delete all, others can only delete their own
        if (currentUserRole != "Admin" && blog.AuthorId != currentUserId)
            throw new UnauthorizedAccessException("You can only delete your own blogs");

        await _blogRepository.DeleteAsync(id, ct);
    }

    public async Task PublishAsync(int id, int currentUserId, string currentUserRole, CancellationToken ct = default)
    {
        // Permission check: Only Admin and Doctor can publish
        if (currentUserRole != "Admin" && currentUserRole != "Doctor")
            throw new UnauthorizedAccessException("Only Admin and Doctor can publish blogs");

        var blog = await _blogRepository.GetByIdAsync(id, ct);
        if (blog == null)
            throw new KeyNotFoundException($"Blog with ID {id} not found");

        // Permission check: Doctor can only publish their own blogs
        if (currentUserRole == "Doctor" && blog.AuthorId != currentUserId)
            throw new UnauthorizedAccessException("You can only publish your own blogs");

        blog.Status = "Published";
        blog.PublishedAt = blog.PublishedAt ?? DateTime.Now;

        await _blogRepository.UpdateAsync(blog, ct);
    }

    public async Task UnpublishAsync(int id, int currentUserId, string currentUserRole, CancellationToken ct = default)
    {
        // Permission check: Only Admin and Doctor can unpublish
        if (currentUserRole != "Admin" && currentUserRole != "Doctor")
            throw new UnauthorizedAccessException("Only Admin and Doctor can unpublish blogs");

        var blog = await _blogRepository.GetByIdAsync(id, ct);
        if (blog == null)
            throw new KeyNotFoundException($"Blog with ID {id} not found");

        // Permission check: Doctor can only unpublish their own blogs
        if (currentUserRole == "Doctor" && blog.AuthorId != currentUserId)
            throw new UnauthorizedAccessException("You can only unpublish your own blogs");

        blog.Status = "Draft";
        blog.PublishedAt = null;

        await _blogRepository.UpdateAsync(blog, ct);
    }

    public async Task IncrementViewCountAsync(int id, CancellationToken ct = default)
    {
        await _blogRepository.IncrementViewCountAsync(id, ct);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeBlogId = null, CancellationToken ct = default)
    {
        return await _blogRepository.SlugExistsAsync(slug, excludeBlogId, ct);
    }

    private async Task<string> GenerateUniqueSlugAsync(string input, int? excludeBlogId, CancellationToken ct)
    {
        var baseSlug = ConvertToSlug(input);
        var slug = baseSlug;
        var counter = 1;

        while (await _blogRepository.SlugExistsAsync(slug, excludeBlogId, ct))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }

    private static string ConvertToSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Normalize Vietnamese characters
        var normalized = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalized)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                // Replace Vietnamese characters
                var replaced = c switch
                {
                    'à' or 'á' or 'ạ' or 'ả' or 'ã' or 'â' or 'ầ' or 'ấ' or 'ậ' or 'ẩ' or 'ẫ' or 'ă' or 'ằ' or 'ắ' or 'ặ' or 'ẳ' or 'ẵ' => 'a',
                    'è' or 'é' or 'ẹ' or 'ẻ' or 'ẽ' or 'ê' or 'ề' or 'ế' or 'ệ' or 'ể' or 'ễ' => 'e',
                    'ì' or 'í' or 'ị' or 'ỉ' or 'ĩ' => 'i',
                    'ò' or 'ó' or 'ọ' or 'ỏ' or 'õ' or 'ô' or 'ồ' or 'ố' or 'ộ' or 'ổ' or 'ỗ' or 'ơ' or 'ờ' or 'ớ' or 'ợ' or 'ở' or 'ỡ' => 'o',
                    'ù' or 'ú' or 'ụ' or 'ủ' or 'ũ' or 'ư' or 'ừ' or 'ứ' or 'ự' or 'ử' or 'ữ' => 'u',
                    'ỳ' or 'ý' or 'ỵ' or 'ỷ' or 'ỹ' => 'y',
                    'đ' => 'd',
                    'À' or 'Á' or 'Ạ' or 'Ả' or 'Ã' or 'Â' or 'Ầ' or 'Ấ' or 'Ậ' or 'Ẩ' or 'Ẫ' or 'Ă' or 'Ằ' or 'Ắ' or 'Ặ' or 'Ẳ' or 'Ẵ' => 'A',
                    'È' or 'É' or 'Ẹ' or 'Ẻ' or 'Ẽ' or 'Ê' or 'Ề' or 'Ế' or 'Ệ' or 'Ể' or 'Ễ' => 'E',
                    'Ì' or 'Í' or 'Ị' or 'Ỉ' or 'Ĩ' => 'I',
                    'Ò' or 'Ó' or 'Ọ' or 'Ỏ' or 'Õ' or 'Ô' or 'Ồ' or 'Ố' or 'Ộ' or 'Ổ' or 'Ỗ' or 'Ơ' or 'Ờ' or 'Ớ' or 'Ợ' or 'Ở' or 'Ỡ' => 'O',
                    'Ù' or 'Ú' or 'Ụ' or 'Ủ' or 'Ũ' or 'Ư' or 'Ừ' or 'Ứ' or 'Ự' or 'Ử' or 'Ữ' => 'U',
                    'Ỳ' or 'Ý' or 'Ỵ' or 'Ỷ' or 'Ỹ' => 'Y',
                    'Đ' => 'D',
                    _ => c
                };
                stringBuilder.Append(replaced);
            }
        }

        var result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

        // Convert to lowercase
        result = result.ToLowerInvariant();

        // Replace spaces and special characters with hyphens
        result = Regex.Replace(result, @"[^a-z0-9\s-]", "");
        result = Regex.Replace(result, @"\s+", "-");
        result = Regex.Replace(result, @"-+", "-");
        result = result.Trim('-');

        // Limit length
        if (result.Length > 500)
        {
            result = result.Substring(0, 500).TrimEnd('-');
        }

        return result;
    }

    private static BlogDto ToDto(Blog blog, int commentCount, string? authorName = null)
    {
        return new BlogDto
        {
            BlogId = blog.BlogId,
            Title = blog.Title,
            Slug = blog.Slug,
            Content = blog.Content,
            Excerpt = blog.Excerpt,
            FeaturedImage = blog.FeaturedImage,
            AuthorId = blog.AuthorId,
            AuthorName = authorName,
            Status = blog.Status,
            PublishedAt = blog.PublishedAt,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
            ViewCount = blog.ViewCount,
            CommentCount = commentCount,
        };
    }
}

