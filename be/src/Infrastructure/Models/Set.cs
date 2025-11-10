using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Set
{
    public long Id { get; set; }

    public string Key { get; set; } = null!;

    public double Score { get; set; }

    public string Value { get; set; } = null!;

    public DateTime? Expireat { get; set; }

    public int Updatecount { get; set; }
}
