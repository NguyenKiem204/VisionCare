using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class ServicesDetail
{
    public int ServiceDetailId { get; set; }

    public int? ServiceTypeId { get; set; }

    public int? ServiceId { get; set; }

    public decimal Cost { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Service? Service { get; set; }

    public virtual ServicesType? ServiceType { get; set; }
}
