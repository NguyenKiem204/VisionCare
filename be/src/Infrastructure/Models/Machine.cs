using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Machine
{
    public int MachineId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? Specifications { get; set; }

    public string? Status { get; set; }
}
