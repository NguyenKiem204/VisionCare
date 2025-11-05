using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Interfaces.Users;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users));
    }

    [HttpGet("search")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> SearchUsers(
        [FromQuery] string? keyword,
        [FromQuery] int? roleId,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool desc = false
    )
    {
        var result = await _userService.SearchUsersAsync(
            keyword,
            roleId,
            status,
            page,
            pageSize,
            sortBy,
            desc
        );
        return Ok(PagedResponse<UserDto>.Ok(result.items, result.totalCount, page, pageSize));
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "OwnProfileOrAdmin")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(ApiResponse<UserDto>.Fail($"User with ID {id} not found."));
        }
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var user = await _userService.CreateUserAsync(request);
        return CreatedAtAction(
            nameof(GetUserById),
            new { id = user.Id },
            ApiResponse<UserDto>.Ok(user)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var user = await _userService.UpdateUserAsync(id, request);
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<UserDto>.Fail($"User with ID {id} not found."));
        }
        return Ok(ApiResponse<UserDto>.Ok(null, "User deleted successfully"));
    }

    [HttpGet("email/{email}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
        {
            return NotFound(ApiResponse<UserDto>.Fail($"User with email {email} not found."));
        }
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpGet("username/{username}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _userService.GetUserByUsernameAsync(username);
        if (user == null)
        {
            return NotFound(ApiResponse<UserDto>.Fail($"User with username {username} not found."));
        }
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpPut("{id}/activate")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> ActivateUser(int id)
    {
        var user = await _userService.ActivateUserAsync(id);
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpPut("{id}/deactivate")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeactivateUser(int id)
    {
        var user = await _userService.DeactivateUserAsync(id);
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpPut("{id}/password")]
    [Authorize(Policy = "OwnProfileOrAdmin")]
    public async Task<IActionResult> ChangePassword(
        int id,
        [FromBody] ChangePasswordRequest request
    )
    {
        var user = await _userService.ChangePasswordAsync(id, request.NewPassword);
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpPut("{id}/role")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateUserRole(
        int id,
        [FromBody] UpdateUserRoleRequest request
    )
    {
        var user = await _userService.UpdateUserRoleAsync(id, request.RoleId);
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpGet("role/{roleId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetUsersByRole(int roleId)
    {
        var users = await _userService.GetUsersByRoleAsync(roleId);
        return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users));
    }

    [HttpGet("active")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetActiveUsers()
    {
        var users = await _userService.GetActiveUsersAsync();
        return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users));
    }

    [HttpGet("inactive")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetInactiveUsers()
    {
        var users = await _userService.GetInactiveUsersAsync();
        return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(users));
    }

    [HttpGet("statistics")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetUserStatistics()
    {
        var totalCount = await _userService.GetTotalUsersCountAsync();
        var activeCount = await _userService.GetActiveUsersCountAsync();
        var roleStats = await _userService.GetUsersByRoleStatsAsync();
        var statusStats = await _userService.GetUsersByStatusStatsAsync();

        return Ok(
            ApiResponse<object>.Ok(
                new
                {
                    TotalCount = totalCount,
                    ActiveCount = activeCount,
                    RoleStats = roleStats,
                    StatusStats = statusStats,
                }
            )
        );
    }
}
