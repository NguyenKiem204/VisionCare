using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Sectioncontent
{
    public string SectionKey { get; set; } = null!;

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }

    public string? MoreData { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
