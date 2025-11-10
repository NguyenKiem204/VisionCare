using System;

namespace VisionCare.Application.DTOs.BlogDto;

public class BlogDto
{
    public int BlogId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Excerpt { get; set; }
    public string? FeaturedImage { get; set; }
    public int AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ViewCount { get; set; }
    public int CommentCount { get; set; }
}

public class CreateBlogRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Excerpt { get; set; }
    public string? FeaturedImage { get; set; }
    public string? Status { get; set; } = "Draft";
}

public class UpdateBlogRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Excerpt { get; set; }
    public string? FeaturedImage { get; set; }
    public string? Status { get; set; }
}

public class BlogSearchRequest
{
    public string? Keyword { get; set; }
    public int? AuthorId { get; set; }
    public string? Status { get; set; }
    public DateTime? PublishedFrom { get; set; }
    public DateTime? PublishedTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

