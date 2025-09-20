using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class ImagesVideo
{
    public int ImageId { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageDescription { get; set; }

    public int? ImageTypeId { get; set; }

    public virtual ImagesType? ImageType { get; set; }
}
