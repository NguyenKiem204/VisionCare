using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Servicestype
{
    public int ServiceTypeId { get; set; }

    public string Name { get; set; } = null!;

    public short DurationMinutes { get; set; }

    public virtual ICollection<Servicesdetail> Servicesdetails { get; set; } = new List<Servicesdetail>();

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}
