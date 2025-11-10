using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class CommentBlog : BaseEntity
{
    public int CommentId { get; set; }
    public int BlogId { get; set; }
    public int? AuthorId { get; set; }
    public int? ParentCommentId { get; set; }
    public string CommentText { get; set; } = string.Empty;
    public string? Status { get; set; } // Active, Hidden, Deleted
    public DateTime? CreatedAt { get; set; }
}

