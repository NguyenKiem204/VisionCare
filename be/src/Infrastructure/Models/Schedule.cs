using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int DoctorId { get; set; }

    public int SlotId { get; set; }

    public DateOnly ScheduleDate { get; set; }

    public string? Status { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Slot Slot { get; set; } = null!;
}
