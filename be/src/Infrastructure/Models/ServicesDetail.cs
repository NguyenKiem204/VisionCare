using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Servicesdetail
{
    public int ServiceDetailId { get; set; }

    public int ServiceId { get; set; }

    public int ServiceTypeId { get; set; }

    public decimal Cost { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Service Service { get; set; } = null!;

    public virtual Servicestype ServiceType { get; set; } = null!;
}
