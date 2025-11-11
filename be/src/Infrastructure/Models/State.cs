using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class State
{
    public long Id { get; set; }

    public long Jobid { get; set; }

    public string Name { get; set; } = null!;

    public string? Reason { get; set; }

    public DateTime Createdat { get; set; }

    public string? Data { get; set; }

    public int Updatecount { get; set; }

    public virtual Job Job { get; set; } = null!;
}
