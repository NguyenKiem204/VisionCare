using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Order : BaseEntity
{
    public int EncounterId { get; set; }
    public string OrderType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Requested";
    public string? ResultUrl { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}


