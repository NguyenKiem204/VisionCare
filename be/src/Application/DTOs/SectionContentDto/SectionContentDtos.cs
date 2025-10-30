namespace VisionCare.Application.DTOs.SectionContentDto;

public class SectionContentDto
{
    public string SectionKey { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public string? MoreData { get; set; }
}

public class SectionContentUpsertDto
{
    public string SectionKey { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public string? MoreData { get; set; }
}


