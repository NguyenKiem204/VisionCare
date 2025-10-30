using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class SectionContent : BaseEntity
{
    public string SectionKey { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public string? MoreData { get; set; }
}


