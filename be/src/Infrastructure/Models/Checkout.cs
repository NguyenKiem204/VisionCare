using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Checkout
{
    public int CheckoutId { get; set; }

    public int AppointmentId { get; set; }

    public string? TransactionType { get; set; }

    public string? TransactionStatus { get; set; }

    public decimal TotalAmount { get; set; }

    public string? TransactionCode { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Notes { get; set; }

    public string? PaymentGateway { get; set; }

    public string? GatewayTransactionId { get; set; }

    public string? GatewayResponse { get; set; }

    public DateTime? RefundRequestedAt { get; set; }

    public DateTime? RefundCompletedAt { get; set; }

    public decimal? RefundAmount { get; set; }

    public string? RefundReason { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
}
