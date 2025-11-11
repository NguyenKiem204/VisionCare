namespace VisionCare.Application.Interfaces.Payment;

/// <summary>
/// Service interface cho VNPay payment gateway
/// </summary>
public interface IVNPayService
{
    /// <summary>
    /// Tạo payment URL từ VNPay
    /// </summary>
    /// <param name="amount">Số tiền (VND)</param>
    /// <param name="orderInfo">Thông tin đơn hàng</param>
    /// <param name="orderId">Mã đơn hàng (appointment code)</param>
    /// <param name="returnUrl">URL để VNPay redirect về sau khi thanh toán</param>
    /// <param name="ipAddress">IP của khách hàng</param>
    /// <returns>Payment URL để redirect</returns>
    Task<string> CreatePaymentUrlAsync(
        decimal amount,
        string orderInfo,
        string orderId,
        string returnUrl,
        string ipAddress
    );

    /// <summary>
    /// Verify payment callback từ VNPay
    /// </summary>
    /// <param name="queryParams">Query parameters từ VNPay callback</param>
    /// <returns>VNPayCallbackResult chứa thông tin thanh toán</returns>
    Task<VNPayCallbackResult> VerifyCallbackAsync(Dictionary<string, string> queryParams);

    /// <summary>
    /// Process refund
    /// </summary>
    Task<bool> ProcessRefundAsync(string transactionId, decimal amount, string reason);
}

public class VNPayCallbackResult
{
    public bool IsSuccess { get; set; }
    public string TransactionId { get; set; } = string.Empty; // VNPay transaction reference
    public decimal Amount { get; set; }
    public string OrderId { get; set; } = string.Empty; // Appointment code
    public string PaymentTime { get; set; } = string.Empty;
    public string? ResponseCode { get; set; }
    public string? Message { get; set; }
}
