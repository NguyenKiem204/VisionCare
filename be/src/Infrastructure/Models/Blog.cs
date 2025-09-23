using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string? BlogContent { get; set; }

    public int? AuthorId { get; set; }

    public DateTime? CreatedDateBlog { get; set; }

    public string? TitleMeta { get; set; }

    public string? TitleImageBlog { get; set; }

    public virtual Account? Author { get; set; }

    public virtual ICollection<Commentblog> Commentblogs { get; set; } = new List<Commentblog>();
}
