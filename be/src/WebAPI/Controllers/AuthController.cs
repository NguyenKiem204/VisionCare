using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.Auth;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Interfaces.Auth;
using VisionCare.Application.Interfaces.Users;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly IWebHostEnvironment _env;

    public AuthController(
        IAuthService authService,
        IUserService userService,
        IWebHostEnvironment env
    )
    {
        _authService = authService;
        _userService = userService;
        _env = env;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        var tokens = await _authService.LoginAsync(request, ip);
        SetRefreshTokenCookie(tokens.RefreshToken);
        var response = new TokenResponse
        {
            AccessToken = tokens.AccessToken,
            ExpiresAt = tokens.ExpiresAt,
            RefreshToken = _env.IsDevelopment() ? tokens.RefreshToken : string.Empty,
        };

        return Ok(ApiResponse<TokenResponse>.Ok(response));
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _authService.RegisterAsync(request);
            return Ok(ApiResponse<object>.Ok(user, "User registered successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(ApiResponse<object>.Fail("Refresh token not found"));
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        var tokens = await _authService.RefreshTokenAsync(refreshToken, ip);
        SetRefreshTokenCookie(tokens.RefreshToken);
        var response = new TokenResponse
        {
            AccessToken = tokens.AccessToken,
            ExpiresAt = tokens.ExpiresAt,
            RefreshToken = _env.IsDevelopment() ? tokens.RefreshToken : string.Empty,
        };

        return Ok(ApiResponse<TokenResponse>.Ok(response));
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            return Unauthorized(ApiResponse<object>.Fail("Refresh token not provided"));
        }

        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        var tokens = await _authService.RefreshTokenAsync(request.RefreshToken, ip);
        SetRefreshTokenCookie(tokens.RefreshToken);
        var response = new TokenResponse
        {
            AccessToken = tokens.AccessToken,
            ExpiresAt = tokens.ExpiresAt,
            RefreshToken = _env.IsDevelopment() ? tokens.RefreshToken : string.Empty,
        };

        return Ok(ApiResponse<TokenResponse>.Ok(response));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(ApiResponse<object>.Fail("Invalid user token"));
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(ApiResponse<object>.Fail("User not found"));
            }

            return Ok(ApiResponse<UserDto>.Ok(user));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            await _authService.RevokeTokenAsync(refreshToken, ip);
        }

        Response.Cookies.Delete("refreshToken");

        return Ok(ApiResponse<object>.Ok(null, "Logged out"));
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/",
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
