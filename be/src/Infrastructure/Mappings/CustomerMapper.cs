using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class CustomerMapper
{
    public static Customer ToDomain(VisionCare.Infrastructure.Models.Customer model)
    {
        return new Customer
        {
            Id = model.AccountId,
            AccountId = model.AccountId,
            CustomerName = model.FullName ?? string.Empty,
            Gender = model.Gender ?? "Unknown",
            Dob = model.Dob,
            Address = model.Address ?? string.Empty,
            Phone = model.Phone,
            Avatar = model.Avatar,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            Account =
                model.Account != null
                    ? new User
                    {
                        Id = model.Account.AccountId,
                        Email = model.Account.Email,
                        Username = model.Account.Username,
                    }
                    : null,
        };
    }

    public static VisionCare.Infrastructure.Models.Customer ToInfrastructure(Customer domain)
    {
        return new VisionCare.Infrastructure.Models.Customer
        {
            AccountId = domain.AccountId ?? 0,
            FullName = domain.CustomerName ?? string.Empty,
            Gender = domain.Gender,
            Dob = domain.Dob,
            Address = domain.Address,
            Phone = domain.Phone,
            Avatar = domain.Avatar,
        };
    }
}
