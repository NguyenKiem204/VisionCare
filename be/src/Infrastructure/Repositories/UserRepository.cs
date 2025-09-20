using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Models;
using DomainRole = VisionCare.Domain.Entities.Role;

namespace VisionCare.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly VisionCareDbContext _context;

    public UserRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context
            .Accounts.Include(a => a.Role)
            .Select(a => new User
            {
                Id = a.AccountId,
                Username = a.Username,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                CreatedDate = a.CreatedDate,
                RoleId = a.RoleId,
                GoogleId = a.GoogleId,
                FacebookId = a.FacebookId,
                FirstConfirm = a.FirstConfirm,
                StatusAccount = a.StatusAccount,
                Role =
                    a.Role != null
                        ? new DomainRole { Id = a.Role.RoleId, RoleName = a.Role.RoleName }
                        : null,
            })
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var account = await _context
            .Accounts.Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.AccountId == id);

        if (account == null)
            return null;

        return new User
        {
            Id = account.AccountId,
            Username = account.Username,
            Email = account.Email,
            PhoneNumber = account.PhoneNumber,
            CreatedDate = account.CreatedDate,
            RoleId = account.RoleId,
            GoogleId = account.GoogleId,
            FacebookId = account.FacebookId,
            FirstConfirm = account.FirstConfirm,
            StatusAccount = account.StatusAccount,
            Role =
                account.Role != null
                    ? new DomainRole { Id = account.Role.RoleId, RoleName = account.Role.RoleName }
                    : null,
        };
    }

    public async Task<User> AddAsync(User user)
    {
        var account = new Account
        {
            Username = user.Username,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            CreatedDate =
                user.CreatedDate ?? DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),
            RoleId = user.RoleId,
            GoogleId = user.GoogleId,
            FacebookId = user.FacebookId,
            FirstConfirm = user.FirstConfirm,
            StatusAccount = user.StatusAccount,
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        user.Id = account.AccountId;
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var account = await _context.Accounts.FindAsync(user.Id);
        if (account != null)
        {
            account.Username = user.Username;
            account.Email = user.Email;
            account.PhoneNumber = user.PhoneNumber;
            account.RoleId = user.RoleId;
            account.GoogleId = user.GoogleId;
            account.FacebookId = user.FacebookId;
            account.FirstConfirm = user.FirstConfirm;
            account.StatusAccount = user.StatusAccount;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account != null)
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}
