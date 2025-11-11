namespace VisionCare.Application.Interfaces.Payment;

public interface ICheckoutService
{
    /// <summary>
    /// Create a checkout record for an appointment payment
    /// </summary>
    Task<int> CreateCheckoutAsync(CreateCheckoutRequest request);
}

public class CreateCheckoutRequest
{
    public int AppointmentId { get; set; }
    public string TransactionType { get; set; } = "VNPay";
    public string TransactionStatus { get; set; } = "Pending";
    public decimal TotalAmount { get; set; }
    public string TransactionCode { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string PaymentGateway { get; set; } = "VNPay";
    public string? GatewayTransactionId { get; set; }
    public string? GatewayResponse { get; set; }
}
