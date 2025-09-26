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
}
