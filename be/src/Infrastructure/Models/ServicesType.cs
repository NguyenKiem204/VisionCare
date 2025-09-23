using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class ServicesType
{
    public int ServiceTypeId { get; set; }

    public string? ServiceTypeName { get; set; }

    public string? DurationService { get; set; }

    public virtual ICollection<ServicesDetail> ServicesDetails { get; set; } = new List<ServicesDetail>();

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}
