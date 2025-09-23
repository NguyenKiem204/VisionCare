using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Discount
{
    public int Discountid { get; set; }

    public string? Discountname { get; set; }

    public int? Percent { get; set; }

    public int? Rankid { get; set; }

    public DateOnly? Enddate { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Customerrank? Rank { get; set; }
}
