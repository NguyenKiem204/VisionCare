using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int EncounterId { get; set; }

    public string OrderType { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? ResultUrl { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Encounter Encounter { get; set; } = null!;
}
