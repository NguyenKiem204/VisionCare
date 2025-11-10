using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Hash
{
    public long Id { get; set; }

    public string Key { get; set; } = null!;

    public string Field { get; set; } = null!;

    public string? Value { get; set; }

    public DateTime? Expireat { get; set; }

    public int Updatecount { get; set; }
}
