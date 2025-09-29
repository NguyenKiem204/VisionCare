using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Models;
using Bcrypt = BCrypt.Net.BCrypt;
using DomainRole = VisionCare.Domain.Entities.Role;

namespace VisionCare.Infrastructure.Mappings;

public static class UserMapper
{
    private static DateTime? ToUnspecified(DateTime? value) =>
        value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Unspecified) : null;

    public static User ToDomain(Account account)
    {
        if (account == null)
            return null!;

        return new User
        {
            Id = account.AccountId,
            Email = account.Email,
            Username = account.Username,
            Password = account.PasswordHash,
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

    public static Account ToInfrastructure(User user)
    {
        if (user == null)
            return null!;

        return new Account
        {
            AccountId = user.Id,
            Email = user.Email,
            Username = user.Username,
            PasswordHash = string.IsNullOrWhiteSpace(user.Password)
                ? null
                : Bcrypt.HashPassword(user.Password),
            EmailConfirmed = user.FirstConfirm == "Confirmed",
            CreatedAt =
                ToUnspecified(user.CreatedDate)
                ?? DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),
            RoleId = user.RoleId ?? 1,
            GoogleId = user.GoogleId,
            FacebookId = user.FacebookId,
            Status = user.StatusAccount,
            UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),
        };
    }

    public static void UpdateAccount(Account account, User user)
    {
        if (account == null || user == null)
            return;

        account.Email = user.Email;
        account.Username = user.Username;
        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            account.PasswordHash = Bcrypt.HashPassword(user.Password);
        }
        account.EmailConfirmed = user.FirstConfirm == "Confirmed";
        account.RoleId = user.RoleId ?? 1;
        account.GoogleId = user.GoogleId;
        account.FacebookId = user.FacebookId;
        account.Status = user.StatusAccount;
        account.LastLogin = ToUnspecified(user.CreatedDate); // set if you pass it via domain
        account.LastPasswordChange = ToUnspecified(account.LastPasswordChange);
        account.LockoutEnd = ToUnspecified(account.LockoutEnd);
        account.PasswordResetExpires = ToUnspecified(account.PasswordResetExpires);
        account.UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
    }
}
