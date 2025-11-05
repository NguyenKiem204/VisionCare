using Microsoft.Extensions.Configuration;
using VisionCare.Application.Interfaces.Payment;

namespace VisionCare.Infrastructure.Services.Payment;

public class PaymentConfiguration : IPaymentConfiguration
{
    private readonly IConfiguration _configuration;

    public PaymentConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetReturnUrl()
    {
        return _configuration["Payment:VNPay:ReturnUrl"]
            ?? throw new InvalidOperationException("Payment:VNPay:ReturnUrl not configured");
    }
}
