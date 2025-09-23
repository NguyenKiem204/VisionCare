using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Banner
{
    public int BannerId { get; set; }

    public string? BannerName { get; set; }

    public string? BannerTitle { get; set; }

    public string? BannerDescription { get; set; }

    public string? BannerStatus { get; set; }

    public string? LinkBanner { get; set; }

    public string? HrefBanner { get; set; }
}
