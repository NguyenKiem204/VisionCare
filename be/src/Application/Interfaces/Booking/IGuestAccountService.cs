using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Booking;

/// <summary>
/// Service for creating guest accounts (Account + Customer) for booking
/// This is needed when a user books without being logged in
/// </summary>
public interface IGuestAccountService
{
    /// <summary>
    /// Find or create a guest account with customer profile
    /// </summary>
    /// <param name="email">Email address (required)</param>
    /// <param name="phone">Phone number (optional)</param>
    /// <param name="customerName">Customer name (optional, defaults to "Khách hàng")</param>
    /// <returns>Customer entity with AccountId set</returns>
    Task<Customer> FindOrCreateGuestCustomerAsync(
        string email,
        string? phone = null,
        string? customerName = null
    );
}
