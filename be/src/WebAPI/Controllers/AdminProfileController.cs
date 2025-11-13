using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Users;
using VisionCare.WebAPI.Responses;
using VisionCare.WebAPI.Utils;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/admin/me")]
[Authorize(Policy = "AdminOnly")]
public class AdminProfileController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IS3StorageService _storage;

    public AdminProfileController(IUserService userService, IS3StorageService storage)
    {
        _userService = userService;
        _storage = storage;
    }

    private int GetCurrentAccountId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("account_id")?.Value
            ?? User.FindFirst("sub")?.Value;
        if (!int.TryParse(idClaim, out var accountId))
        {
            throw new UnauthorizedAccessException("Invalid or missing account id claim.");
        }
        return accountId;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetMyProfile()
    {
        var accountId = GetCurrentAccountId();
        var user = await _userService.GetUserByIdAsync(accountId);
        if (user == null)
        {
            return NotFound(ApiResponse<UserDto>.Fail("User profile not found."));
        }
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> UpdateMyProfile([FromBody] UpdateUserRequest request)
    {
        var accountId = GetCurrentAccountId();
        var updated = await _userService.UpdateUserAsync(accountId, request);
        return Ok(ApiResponse<UserDto>.Ok(updated));
    }
}

