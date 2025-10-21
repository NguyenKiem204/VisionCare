using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;

namespace VisionCare.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly VisionCareDbContext _context;

    public RoleRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        var roles = await _context.Roles.ToListAsync();
        return roles.Select(MapToDomain);
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);

        return role != null ? MapToDomain(role) : null;
    }

    public async Task<Role?> GetByNameAsync(string roleName)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);

        return role != null ? MapToDomain(role) : null;
    }

    public async Task<Role> AddAsync(Role role)
    {
        var roleModel = MapToModel(role);
        _context.Roles.Add(roleModel);
        await _context.SaveChangesAsync();
        return MapToDomain(roleModel);
    }

    public async Task UpdateAsync(Role role)
    {
        var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == role.Id);

        if (existingRole != null)
        {
            existingRole.RoleName = role.RoleName;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Role>> SearchAsync(string keyword)
    {
        var roles = await _context.Roles.Where(r => r.RoleName.Contains(keyword)).ToListAsync();

        return roles.Select(MapToDomain);
    }

    public async Task<bool> IsInUseAsync(int roleId)
    {
        return await _context.Accounts.AnyAsync(a => a.RoleId == roleId);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Roles.CountAsync();
    }

    public async Task<Dictionary<string, int>> GetUsageStatsAsync()
    {
        return await _context
            .Roles.GroupBy(r => r.RoleName)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    private static Role MapToDomain(VisionCare.Infrastructure.Models.Role model)
    {
        return new Role
        {
            Id = model.RoleId,
            RoleName = model.RoleName,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };
    }

    private static VisionCare.Infrastructure.Models.Role MapToModel(Role domain)
    {
        return new Infrastructure.Models.Role
        {
            RoleId = domain.Id,
            RoleName = domain.RoleName,
            CreatedAt = domain.Created,
        };
    }
}
