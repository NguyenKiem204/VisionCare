using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Models;
using DomainRole = VisionCare.Domain.Entities.Role;

namespace VisionCare.Infrastructure.Mappings;

public static class UserMapper
{
    /// <summary>
    /// Maps Infrastructure Account to Domain User
    /// </summary>
    public static User ToDomain(Account account)
    {
        if (account == null)
            return null!;

        return new User
        {
            Id = account.AccountId,
            Email = account.Email,
            Username = account.Username,
            Password = account.PasswordHash, // Map password hash to password for domain
            CreatedDate = account.CreatedAt,
            RoleId = account.RoleId,
            GoogleId = account.GoogleId,
            FacebookId = account.FacebookId,
            FirstConfirm = account.EmailConfirmed == true ? "Confirmed" : "Pending",
            StatusAccount = account.Status,
            Role =
                account.Role != null
                    ? new DomainRole { Id = account.Role.RoleId, RoleName = account.Role.RoleName }
                    : null,
        };
    }

    /// <summary>
    /// Maps Domain User to Infrastructure Account
    /// </summary>
    public static Account ToInfrastructure(User user)
    {
        if (user == null)
            return null!;

        return new Account
        {
            AccountId = user.Id,
            Email = user.Email,
            Username = user.Username,
            PasswordHash = user.Password,
            EmailConfirmed = user.FirstConfirm == "Confirmed",
            CreatedAt = user.CreatedDate ?? DateTime.UtcNow,
            RoleId = user.RoleId ?? 1, // Default role if null
            GoogleId = user.GoogleId,
            FacebookId = user.FacebookId,
            Status = user.StatusAccount,
            UpdatedAt = DateTime.UtcNow,
        };
    }

    /// <summary>
    /// Updates existing Account with User data
    /// </summary>
    public static void UpdateAccount(Account account, User user)
    {
        if (account == null || user == null)
            return;

        account.Email = user.Email;
        account.Username = user.Username;
        account.PasswordHash = user.Password;
        account.EmailConfirmed = user.FirstConfirm == "Confirmed";
        account.RoleId = user.RoleId ?? 1;
        account.GoogleId = user.GoogleId;
        account.FacebookId = user.FacebookId;
        account.Status = user.StatusAccount;
        account.UpdatedAt = DateTime.UtcNow;
    }
}
