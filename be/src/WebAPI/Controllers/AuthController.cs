using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.Commands.Auth;
using VisionCare.Application.DTOs.Auth;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        var tokens = await _mediator.Send(
            new LoginCommand
            {
                Email = request.Email,
                Password = request.Password,
                IpAddress = ip,
            }
        );
        return Ok(ApiResponse<TokenResponse>.Ok(tokens));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        var tokens = await _mediator.Send(
            new RefreshTokenCommand { RefreshToken = refreshToken, IpAddress = ip }
        );
        return Ok(ApiResponse<TokenResponse>.Ok(tokens));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string refreshToken)
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
        await _mediator.Send(new LogoutCommand { RefreshToken = refreshToken, IpAddress = ip });
        return Ok(ApiResponse<object>.Ok(null, "Logged out"));
    }
}
