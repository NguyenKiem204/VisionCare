using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Banner
{
    public int BannerId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? LinkUrl { get; set; }

    public int? DisplayOrder { get; set; }

    public string? Status { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }
}
