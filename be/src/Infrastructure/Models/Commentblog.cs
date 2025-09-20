using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Commentblog
{
    public int CommentBlogId { get; set; }

    public string? Comment { get; set; }

    public int? AuthorId { get; set; }

    public int? Tuongtac { get; set; }

    public int? ParentCommentId { get; set; }

    public int? BlogId { get; set; }

    public virtual Account? Author { get; set; }

    public virtual Blog? Blog { get; set; }
}
