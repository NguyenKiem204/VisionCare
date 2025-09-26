using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Imagesservice
{
    public int ServiceId { get; set; }

    public string? ImageMain { get; set; }

    public string? ImageBefore { get; set; }

    public string? ImageAfter { get; set; }

    public virtual Service Service { get; set; } = null!;
}
