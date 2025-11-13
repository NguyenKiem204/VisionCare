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

        _tmnCode =
            _configuration["Payment:VNPay:TmnCode"]?.Trim()
            ?? throw new InvalidOperationException("VNPay TmnCode not configured");
        _hashSecret =
            _configuration["Payment:VNPay:HashSecret"]?.Trim()
            ?? throw new InvalidOperationException("VNPay HashSecret not configured");

        logger.LogInformation(
            "VNPay configured: TmnCode={TmnCode}, HashSecret length={Length}",
            _tmnCode,
            _hashSecret.Length
        );
        _url =
            _configuration["Payment:VNPay:Url"]
            ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        _returnUrl =
            _configuration["Payment:VNPay:ReturnUrl"]
            ?? throw new InvalidOperationException("VNPay ReturnUrl not configured");
    }

    public async Task<string> CreatePaymentUrlAsync(
        decimal amount,
        string orderInfo,
        string orderId,
        string returnUrl,
        string ipAddress
    )
    {
        var effectiveReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? _returnUrl : returnUrl;

        // VNPay requires GMT+7 timestamps (Vietnam time)
        var vnNow = GetVietnamTimeNow();
        var createDate = vnNow;
        var expireDate = vnNow.AddMinutes(15);

        var vnpParams = new Dictionary<string, string>
        {
            { "vnp_Version", _version },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", _tmnCode },
            {
                "vnp_Amount",
                ((long)Math.Round(amount * 100M, 0, MidpointRounding.AwayFromZero)).ToString()
            },
            { "vnp_CurrCode", "VND" },
            { "vnp_TxnRef", orderId },
            { "vnp_OrderInfo", orderInfo },
            { "vnp_OrderType", "other" },
            { "vnp_Locale", "vn" },
            { "vnp_ReturnUrl", effectiveReturnUrl },
            { "vnp_IpAddr", ipAddress },
            { "vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss") },
            { "vnp_ExpireDate", expireDate.ToString("yyyyMMddHHmmss") },
        };

        // Build hash data EXCLUDING vnp_SecureHash and vnp_SecureHashType
        var hashData = BuildHashDataEncoded(vnpParams);
        _logger.LogInformation("VNPay query string before hash: {QueryString}", hashData);

        var secureHash = HmacSHA512(_hashSecret, hashData);
        _logger.LogInformation("VNPay secure hash: {Hash}", secureHash);
        _logger.LogInformation(
            "VNPay HashSecret prefix (first 8 chars): {HashSecretPrefix}",
            _hashSecret.Substring(0, Math.Min(8, _hashSecret.Length))
        );

        // Add hash fields AFTER computing signature
        vnpParams.Add("vnp_SecureHashType", "HmacSHA512");
        vnpParams.Add("vnp_SecureHash", secureHash);

        var finalQueryString = BuildQueryString(vnpParams);
        var paymentUrl = $"{_url}?{finalQueryString}";

        _logger.LogInformation(
            "Created VNPay payment URL for order {OrderId}. URL length: {Length}, TmnCode: {TmnCode}, HashSecret length: {HashSecretLength}",
            orderId,
            paymentUrl.Length,
            _tmnCode,
            _hashSecret?.Length ?? 0
        );
        _logger.LogInformation("VNPay payment URL: {PaymentUrl}", paymentUrl);

        if (_tmnCode == "YOUR_TMN_CODE" || _hashSecret == "YOUR_HASH_SECRET")
        {
            _logger.LogWarning(
                "VNPay configuration appears to be using placeholder values. Please update TmnCode and HashSecret in appsettings.json"
            );
        }

        return await Task.FromResult(paymentUrl);
    }

    public async Task<VNPayCallbackResult> VerifyCallbackAsync(
        Dictionary<string, string> queryParams
    )
    {
        var vnpParams = queryParams
            .Where(kv => kv.Key.StartsWith("vnp_"))
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        if (!vnpParams.ContainsKey("vnp_SecureHash"))
        {
            return new VNPayCallbackResult { IsSuccess = false, Message = "Missing secure hash" };
        }

        var secureHash = vnpParams["vnp_SecureHash"];
        vnpParams.Remove("vnp_SecureHash");
        if (vnpParams.ContainsKey("vnp_SecureHashType"))
        {
            vnpParams.Remove("vnp_SecureHashType");
        }

        // Rebuild URL-encoded, sorted key=value string for verification
        var hashData = BuildHashDataEncoded(vnpParams);
        var checkSum = HmacSHA512(_hashSecret, hashData);

        if (!string.Equals(secureHash, checkSum, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("VNPay callback signature verification failed");
            return new VNPayCallbackResult { IsSuccess = false, Message = "Invalid signature" };
        }

        var responseCode = vnpParams.GetValueOrDefault("vnp_ResponseCode", "");
        var isSuccess = responseCode == "00";

        return await Task.FromResult(
            new VNPayCallbackResult
            {
                IsSuccess = isSuccess,
                TransactionId = vnpParams.GetValueOrDefault("vnp_TransactionNo", ""),
                Amount = decimal.Parse(vnpParams.GetValueOrDefault("vnp_Amount", "0")) / 100, // Convert from cents
                OrderId = vnpParams.GetValueOrDefault("vnp_TxnRef", ""),
                PaymentTime = vnpParams.GetValueOrDefault("vnp_PayDate", ""),
                ResponseCode = responseCode,
                Message = isSuccess ? "Thanh toán thành công" : "Thanh toán thất bại",
            }
        );
    }

    public async Task<bool> ProcessRefundAsync(string transactionId, decimal amount, string reason)
    {
        // TODO: Implement VNPay refund API call
        // VNPay có API refund, nhưng cần cấu hình thêm
        _logger.LogWarning(
            "VNPay refund not yet implemented for transaction {TransactionId}",
            transactionId
        );
        return await Task.FromResult(false);
    }

    private string BuildQueryString(Dictionary<string, string> vnpParams)
    {
        return string.Join(
            "&",
            vnpParams.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}={FormUrlEncode(kv.Value)}")
        );
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

    private string BuildHashData(Dictionary<string, string> vnpParams)
    {
        return string.Join(
            "&",
            vnpParams
                .Where(kv => !string.IsNullOrEmpty(kv.Value))
                .OrderBy(kv => kv.Key)
                .Select(kv => $"{kv.Key}={kv.Value}")
        );
    }

    private string BuildHashDataEncoded(Dictionary<string, string> vnpParams)
    {
        return string.Join(
            "&",
            vnpParams
                .Where(kv => !string.IsNullOrEmpty(kv.Value))
                .OrderBy(kv => kv.Key)
                .Select(kv => $"{kv.Key}={FormUrlEncode(kv.Value)}")
        );
    }

    private string FormUrlEncode(string value)
    {
        // Emulate application/x-www-form-urlencoded where space => '+'
        // Uri.EscapeDataString produces %20 for spaces; VNPay samples often use '+'.
        return Uri.EscapeDataString(value).Replace("%20", "+");
    }

    private DateTime GetVietnamTimeNow()
    {
        // Prefer IANA id for Linux/docker; fallback to Windows id
        string[] tzIds = new[] { "Asia/Ho_Chi_Minh", "SE Asia Standard Time" };
        foreach (var id in tzIds)
        {
            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(id);
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
            }
            catch
            {
                // continue
            }
        }
        // Fallback: UTC+7 offset if timezone id not found
        return DateTime.UtcNow.AddHours(7);
    }
}
