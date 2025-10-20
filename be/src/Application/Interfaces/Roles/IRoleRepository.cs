using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByNameAsync(string roleName);
    Task<Role> AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(int id);

    // Additional operations for RoleManager
    Task<IEnumerable<Role>> SearchAsync(string keyword);
    Task<bool> IsInUseAsync(int roleId);
    Task<int> GetTotalCountAsync();
    Task<Dictionary<string, int>> GetUsageStatsAsync();
}
