using System.Collections.Generic;

namespace VisionCare.Application.DTOs.SectionContentDto;

public class WhyUsUpsertDto
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public List<string> Bullets { get; set; } = new();
    public List<string> Images { get; set; } = new();
}

public class AboutUpsertDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Image { get; set; }
}
