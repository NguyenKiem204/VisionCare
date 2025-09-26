using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string DiscountName { get; set; } = null!;

    public decimal DiscountPercent { get; set; }

    public int? RankId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Customerrank? Rank { get; set; }
}
