using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Checkout
{
    public int CheckoutId { get; set; }

    public int? AppointmentId { get; set; }

    public string? TransactionType { get; set; }

    public string? TransactionStatus { get; set; }

    public decimal? TotalBill { get; set; }

    public string? CheckoutCode { get; set; }

    public DateTime? CheckoutTime { get; set; }

    public virtual Appointment? Appointment { get; set; }
}
