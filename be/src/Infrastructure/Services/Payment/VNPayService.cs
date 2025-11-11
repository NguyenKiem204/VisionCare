using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VisionCare.Application.Interfaces.Payment;

namespace VisionCare.Infrastructure.Services.Payment;

public class VNPayService : IVNPayService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<VNPayService> _logger;
    private readonly string _tmnCode;
    private readonly string _hashSecret;
    private readonly string _url;
    private readonly string _returnUrl;
    private readonly string _version = "2.1.0";

    public VNPayService(IConfiguration configuration, ILogger<VNPayService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        _tmnCode = _configuration["Payment:VNPay:TmnCode"] 
            ?? throw new InvalidOperationException("VNPay TmnCode not configured");
        _hashSecret = _configuration["Payment:VNPay:HashSecret"]
            ?? throw new InvalidOperationException("VNPay HashSecret not configured");
        _url = _configuration["Payment:VNPay:Url"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        _returnUrl = _configuration["Payment:VNPay:ReturnUrl"] 
            ?? throw new InvalidOperationException("VNPay ReturnUrl not configured");
    }

    public async Task<string> CreatePaymentUrlAsync(
        decimal amount,
        string orderInfo,
        string orderId,
        string returnUrl,
        string ipAddress)
    {
        var vnpParams = new Dictionary<string, string>
        {
            { "vnp_Version", _version },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", _tmnCode },
            { "vnp_Amount", ((long)(amount * 100)).ToString() }, // Convert to cents (VND x 100)
            { "vnp_CurrCode", "VND" },
            { "vnp_TxnRef", orderId },
            { "vnp_OrderInfo", orderInfo },
            { "vnp_OrderType", "other" },
            { "vnp_Locale", "vn" },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_IpAddr", ipAddress },
            { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
        };

        // Build query string and hash
        var queryString = BuildQueryString(vnpParams);
        var secureHash = HmacSHA512(_hashSecret, queryString);
        vnpParams.Add("vnp_SecureHash", secureHash);

        // Final query string
        var finalQueryString = BuildQueryString(vnpParams);
        var paymentUrl = $"{_url}?{finalQueryString}";

        _logger.LogInformation("Created VNPay payment URL for order {OrderId}", orderId);
        return await Task.FromResult(paymentUrl);
    }

    public async Task<VNPayCallbackResult> VerifyCallbackAsync(Dictionary<string, string> queryParams)
    {
        var vnpParams = queryParams.Where(kv => kv.Key.StartsWith("vnp_")).ToDictionary(kv => kv.Key, kv => kv.Value);
        
        if (!vnpParams.ContainsKey("vnp_SecureHash"))
        {
            return new VNPayCallbackResult
            {
                IsSuccess = false,
                Message = "Missing secure hash"
            };
        }

        var secureHash = vnpParams["vnp_SecureHash"];
        vnpParams.Remove("vnp_SecureHash");

        var queryString = BuildQueryString(vnpParams);
        var checkSum = HmacSHA512(_hashSecret, queryString);

        if (secureHash != checkSum)
        {
            _logger.LogWarning("VNPay callback signature verification failed");
            return new VNPayCallbackResult
            {
                IsSuccess = false,
                Message = "Invalid signature"
            };
        }

        var responseCode = vnpParams.GetValueOrDefault("vnp_ResponseCode", "");
        var isSuccess = responseCode == "00";

        return await Task.FromResult(new VNPayCallbackResult
        {
            IsSuccess = isSuccess,
            TransactionId = vnpParams.GetValueOrDefault("vnp_TransactionNo", ""),
            Amount = decimal.Parse(vnpParams.GetValueOrDefault("vnp_Amount", "0")) / 100, // Convert from cents
            OrderId = vnpParams.GetValueOrDefault("vnp_TxnRef", ""),
            PaymentTime = vnpParams.GetValueOrDefault("vnp_PayDate", ""),
            ResponseCode = responseCode,
            Message = isSuccess ? "Thanh toán thành công" : "Thanh toán thất bại"
        });
    }

    public async Task<bool> ProcessRefundAsync(string transactionId, decimal amount, string reason)
    {
        // TODO: Implement VNPay refund API call
        // VNPay có API refund, nhưng cần cấu hình thêm
        _logger.LogWarning("VNPay refund not yet implemented for transaction {TransactionId}", transactionId);
        return await Task.FromResult(false);
    }

    private string BuildQueryString(Dictionary<string, string> vnpParams)
    {
        return string.Join("&", vnpParams.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
    }

    private string HmacSHA512(string key, string inputData)
    {
        var hash = new StringBuilder();
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
        using (var hmac = new HMACSHA512(keyBytes))
        {
            byte[] hashValue = hmac.ComputeHash(inputBytes);
            foreach (byte b in hashValue)
            {
                hash.Append(b.ToString("x2"));
            }
        }
        return hash.ToString();
    }
}
