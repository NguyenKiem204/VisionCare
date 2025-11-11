using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Booking;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Services;

public class GuestAccountService : IGuestAccountService
{
    private readonly VisionCareDbContext _context;

    public GuestAccountService(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> FindOrCreateGuestCustomerAsync(
        string email,
        string? phone = null,
        string? customerName = null
    )
    {
        // Try to find existing account by email
        var existingAccount = await _context.Accounts
            .Include(a => a.Customer)
            .FirstOrDefaultAsync(a => a.Email == email);

        // If account exists and has customer profile, return it
        if (existingAccount?.Customer != null)
        {
            return CustomerMapper.ToDomain(existingAccount.Customer);
        }

        // Create new account if doesn't exist
        if (existingAccount == null)
        {
            existingAccount = new VisionCare.Infrastructure.Models.Account
            {
                Email = email,
                Username = email.Split('@')[0], // Use email prefix as username
                PasswordHash = null, // No password for guest bookings
                EmailConfirmed = false,
                RoleId = 4, // Customer role
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Accounts.Add(existingAccount);
            await _context.SaveChangesAsync();
        }

        // Create customer profile if doesn't exist
        if (existingAccount.Customer == null)
        {
            var customerModel = new VisionCare.Infrastructure.Models.Customer
            {
                AccountId = existingAccount.AccountId,
                FullName = customerName ?? "Khách hàng",
                Phone = phone,
                Gender = null,
                Dob = null,
                Address = null,
                RankId = 1, // Default to Bronze rank
            };

            _context.Customers.Add(customerModel);
            await _context.SaveChangesAsync();

            // Reload with account
            customerModel = await _context.Customers
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.AccountId == existingAccount.AccountId);

            if (customerModel == null)
                throw new InvalidOperationException("Failed to create customer");

            return CustomerMapper.ToDomain(customerModel);
        }

        return CustomerMapper.ToDomain(existingAccount.Customer);
    }
}
