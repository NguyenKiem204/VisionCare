using AutoMapper;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Users;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
    {
        // Validate email uniqueness
        if (await IsEmailExistsAsync(request.Email))
        {
            throw new ValidationException("Email already exists");
        }

        // Validate username uniqueness
        if (
            !string.IsNullOrEmpty(request.Username) && await IsUsernameExistsAsync(request.Username)
        )
        {
            throw new ValidationException("Username already exists");
        }

        // Use AutoMapper to create entity from DTO
        var user = _mapper.Map<User>(request);
        user.StatusAccount = "Active";
        user.CreatedDate = DateTime.UtcNow;

        var createdUser = await _userRepository.AddAsync(user);
        return _mapper.Map<UserDto>(createdUser);
    }

    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        // Validate email uniqueness if changed
        if (existingUser.Email != request.Email && await IsEmailExistsAsync(request.Email))
        {
            throw new ValidationException("Email already exists");
        }

        // Validate username uniqueness if changed
        if (
            existingUser.Username != request.Username
            && !string.IsNullOrEmpty(request.Username)
            && await IsUsernameExistsAsync(request.Username)
        )
        {
            throw new ValidationException("Username already exists");
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingUser);

        await _userRepository.UpdateAsync(existingUser);
        return _mapper.Map<UserDto>(existingUser);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        await _userRepository.DeleteAsync(id);
        return true;
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
        );
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u =>
            u.Username?.Equals(username, StringComparison.OrdinalIgnoreCase) == true
        );
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        var user = await GetUserByEmailAsync(email);
        return user != null;
    }

    public async Task<bool> IsUsernameExistsAsync(string username)
    {
        var user = await GetUserByUsernameAsync(username);
        return user != null;
    }

    public async Task<UserDto> ActivateUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        // Use domain method to activate account
        user.ActivateAccount();
        await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> DeactivateUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        // Use domain method to deactivate account
        user.DeactivateAccount();
        await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> ChangePasswordAsync(int id, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        // Use domain method to change password
        user.ChangePassword(newPassword);
        await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUserRoleAsync(int id, int roleId)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {id} not found");
        }

        // Use domain method to update role
        user.UpdateRole(roleId);
        await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<(IEnumerable<UserDto> items, int totalCount)> SearchUsersAsync(
        string? keyword,
        int? roleId,
        string? status,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var result = await _userRepository.SearchAsync(
            keyword,
            roleId,
            status,
            page,
            pageSize,
            sortBy,
            desc
        );
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(result.items);
        return (userDtos, result.totalCount);
    }

    public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId)
    {
        var users = await _userRepository.GetAllAsync();
        var filteredUsers = users.Where(u => u.RoleId == roleId);
        return _mapper.Map<IEnumerable<UserDto>>(filteredUsers);
    }

    public async Task<IEnumerable<UserDto>> GetActiveUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var activeUsers = users.Where(u => u.StatusAccount == "Active");
        return _mapper.Map<IEnumerable<UserDto>>(activeUsers);
    }

    public async Task<IEnumerable<UserDto>> GetInactiveUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var inactiveUsers = users.Where(u => u.StatusAccount == "Inactive");
        return _mapper.Map<IEnumerable<UserDto>>(inactiveUsers);
    }

    public async Task<int> GetTotalUsersCountAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Count();
    }

    public async Task<int> GetActiveUsersCountAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Count(u => u.StatusAccount == "Active");
    }

    public async Task<Dictionary<string, int>> GetUsersByRoleStatsAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users
            .GroupBy(u => u.Role?.RoleName ?? "No Role")
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetUsersByStatusStatsAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users
            .GroupBy(u => u.StatusAccount ?? "Unknown")
            .ToDictionary(g => g.Key, g => g.Count());
    }
}
