using VisionCare.Application.DTOs.RoleDto;

namespace VisionCare.Application.Interfaces.Roles;

/// <summary>
/// Service interface for role management operations
/// </summary>
public interface IRoleService
{
    // Basic CRUD operations
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleDto?> GetRoleByIdAsync(int id);
    Task<RoleDto?> GetRoleByNameAsync(string roleName);
    Task<RoleDto> CreateRoleAsync(CreateRoleRequest request);
    Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleRequest request);
    Task<bool> DeleteRoleAsync(int id);

    // Business operations
    Task<IEnumerable<RoleDto>> SearchRolesAsync(string keyword);
    Task<bool> IsRoleInUseAsync(int roleId);

    // Statistics
    Task<int> GetTotalRolesCountAsync();
    Task<Dictionary<string, int>> GetRoleUsageStatsAsync();
}
