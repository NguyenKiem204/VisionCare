using System;

namespace VisionCare.Application.DTOs.BannerDto;

public class BannerDto
{
    public int BannerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public int? DisplayOrder { get; set; }
    public string? Status { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    // Future: public string Place { get; set; } = "home_hero";
}

public class CreateBannerRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public int? DisplayOrder { get; set; }
    public string? Status { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}

public class UpdateBannerRequest : CreateBannerRequest { }


