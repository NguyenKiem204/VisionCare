using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Blog : BaseEntity
{
    public int BlogId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Excerpt { get; set; }
    public string? FeaturedImage { get; set; }
    public int AuthorId { get; set; }
    public string? Status { get; set; } // Draft, Published, Archived
    public DateTime? PublishedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ViewCount { get; set; }
}

