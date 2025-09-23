using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Slot
{
    public int SlotId { get; set; }

    public string StartTime { get; set; } = null!;

    public string EndTime { get; set; } = null!;

    public int ServiceTypeId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ServicesType ServiceType { get; set; } = null!;
}
