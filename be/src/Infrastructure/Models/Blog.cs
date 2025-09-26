using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Excerpt { get; set; }

    public string? FeaturedImage { get; set; }

    public int AuthorId { get; set; }

    public string? Status { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ViewCount { get; set; }

    public virtual Account Author { get; set; } = null!;

    public virtual ICollection<Commentblog> Commentblogs { get; set; } = new List<Commentblog>();
}
