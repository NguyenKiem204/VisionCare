using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

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
        var accounts = await _context.Accounts.Include(a => a.Role).ToListAsync();

        return accounts.Select(UserMapper.ToDomain);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var account = await _context
            .Accounts.Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.AccountId == id);

        return account != null ? UserMapper.ToDomain(account) : null;
    }

    public async Task<User> AddAsync(User user)
    {
        var account = UserMapper.ToInfrastructure(user);

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
            UserMapper.UpdateAccount(account, user);
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

    public async Task<(IEnumerable<User> items, int totalCount)> SearchAsync(
        string? keyword,
        int? roleId,
        string? status,
        int page,
        int pageSize,
        string? sortBy,
        bool desc
    )
    {
        var query = _context.Accounts.Include(a => a.Role).AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var lowered = keyword.Trim().ToLower();
            query = query.Where(a =>
                (a.Username != null && a.Username.ToLower().Contains(lowered))
                || a.Email.ToLower().Contains(lowered)
            );
        }

        if (roleId.HasValue)
        {
            query = query.Where(a => a.RoleId == roleId.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(a => a.Status != null && a.Status == status);
        }

        // Sorting
        query = (sortBy?.ToLower()) switch
        {
            "email" => desc ? query.OrderByDescending(a => a.Email) : query.OrderBy(a => a.Email),
            "username" => desc
                ? query.OrderByDescending(a => a.Username)
                : query.OrderBy(a => a.Username),
            "createdat" => desc
                ? query.OrderByDescending(a => a.CreatedAt)
                : query.OrderBy(a => a.CreatedAt),
            "rolename" => desc
                ? query.OrderByDescending(a => a.Role.RoleName)
                : query.OrderBy(a => a.Role.RoleName),
            _ => desc ? query.OrderByDescending(a => a.AccountId) : query.OrderBy(a => a.AccountId),
        };

        var totalCount = await query.CountAsync();

        var skip = (Math.Max(page, 1) - 1) * Math.Max(pageSize, 1);

        var accounts = await query.Skip(skip).Take(Math.Max(pageSize, 1)).ToListAsync();

        var items = accounts.Select(UserMapper.ToDomain);
        return (items, totalCount);
    }
}
