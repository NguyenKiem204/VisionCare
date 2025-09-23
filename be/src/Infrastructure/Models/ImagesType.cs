using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class ImagesType
{
    public int ImageTypeId { get; set; }

    public string? ImageType { get; set; }

    public virtual ICollection<ImagesVideo> ImagesVideos { get; set; } = new List<ImagesVideo>();
}
