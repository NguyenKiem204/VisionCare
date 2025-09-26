using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Contentstory
{
    public int StoryId { get; set; }

    public string? PatientName { get; set; }

    public string? PatientImage { get; set; }

    public string StoryContent { get; set; } = null!;

    public int? DisplayOrder { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }
}
