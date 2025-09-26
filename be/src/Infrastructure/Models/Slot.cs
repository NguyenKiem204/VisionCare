using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Slot
{
    public int SlotId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int ServiceTypeId { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Servicestype ServiceType { get; set; } = null!;
}
