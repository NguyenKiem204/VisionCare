namespace VisionCare.Application.Interfaces.Payment;

/// <summary>
/// Configuration interface for payment gateway
/// Abstracts away IConfiguration dependency from Application layer
/// </summary>
public interface IPaymentConfiguration
{
    string GetReturnUrl();
}
