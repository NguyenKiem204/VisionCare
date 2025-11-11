using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Workshift
{
    public int ShiftId { get; set; }

    public string ShiftName { get; set; } = null!;

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool? IsActive { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Doctorschedule> Doctorschedules { get; set; } = new List<Doctorschedule>();
}
