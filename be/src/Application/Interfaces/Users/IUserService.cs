using VisionCare.Application.DTOs.User;

namespace VisionCare.Application.Interfaces.Users;

/// <summary>
/// Service interface for user management operations
/// </summary>
public interface IUserService
{
    // Basic CRUD operations
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserRequest request);
    Task<UserDto> UpdateUserAsync(int id, UpdateUserRequest request);
    Task<bool> DeleteUserAsync(int id);

    // User lookup operations
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<bool> IsEmailExistsAsync(string email);
    Task<bool> IsUsernameExistsAsync(string username);

    // User status management
    Task<UserDto> ActivateUserAsync(int id);
    Task<UserDto> DeactivateUserAsync(int id);
    Task<UserDto> ChangePasswordAsync(int id, string newPassword);
    Task<UserDto> UpdateUserRoleAsync(int id, int roleId);

    // Search and filtering
    Task<(IEnumerable<UserDto> items, int totalCount)> SearchUsersAsync(
        string? keyword,
        int? roleId,
        string? status,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    );
    Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId);
    Task<IEnumerable<UserDto>> GetActiveUsersAsync();
    Task<IEnumerable<UserDto>> GetInactiveUsersAsync();

    // Statistics
    Task<int> GetTotalUsersCountAsync();
    Task<int> GetActiveUsersCountAsync();
    Task<Dictionary<string, int>> GetUsersByRoleStatsAsync();
    Task<Dictionary<string, int>> GetUsersByStatusStatsAsync();
}
