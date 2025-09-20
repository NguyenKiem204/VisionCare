using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Customerrank
{
    public int Rankid { get; set; }

    public string? Rankname { get; set; }

    public double? Minamount { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}
