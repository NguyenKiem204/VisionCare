using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Weeklyschedule
{
    public int WeeklyScheduleId { get; set; }

    public int DoctorId { get; set; }

    public int DayOfWeek { get; set; }

    public int SlotId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Slot Slot { get; set; } = null!;
}
