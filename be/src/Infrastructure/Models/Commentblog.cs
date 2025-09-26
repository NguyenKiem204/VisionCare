using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Commentblog
{
    public int CommentId { get; set; }

    public int BlogId { get; set; }

    public int? AuthorId { get; set; }

    public int? ParentCommentId { get; set; }

    public string CommentText { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account? Author { get; set; }

    public virtual Blog Blog { get; set; } = null!;

    public virtual ICollection<Commentblog> InverseParentComment { get; set; } = new List<Commentblog>();

    public virtual Commentblog? ParentComment { get; set; }
}
