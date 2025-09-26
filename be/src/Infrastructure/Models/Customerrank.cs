using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Customerrank
{
    public int RankId { get; set; }

    public string RankName { get; set; } = null!;

    public decimal MinAmount { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}
