using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Payment;
using VisionCare.Infrastructure.Data;

namespace VisionCare.Infrastructure.Services.Payment;

public class CheckoutService : ICheckoutService
{
    private readonly VisionCareDbContext _context;

    public CheckoutService(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateCheckoutAsync(CreateCheckoutRequest request)
    {
        var checkout = new VisionCare.Infrastructure.Models.Checkout
        {
            AppointmentId = request.AppointmentId,
            TransactionType = request.TransactionType,
            TransactionStatus = request.TransactionStatus,
            TotalAmount = request.TotalAmount,
            TransactionCode = request.TransactionCode,
            PaymentDate = request.PaymentDate,
            PaymentGateway = request.PaymentGateway,
            GatewayTransactionId = request.GatewayTransactionId,
            GatewayResponse = request.GatewayResponse,
        };

        _context.Checkouts.Add(checkout);
        await _context.SaveChangesAsync();

        return checkout.CheckoutId;
    }
}
