using MediatR;
using VisionCare.Application.DTOs.Auth;
using VisionCare.Application.Interfaces.Auth;

namespace VisionCare.Application.Commands.Auth;

public class LoginCommand : IRequest<TokenResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenResponse>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<TokenResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var dto = new LoginRequest { Email = request.Email, Password = request.Password };
        return await _authService.LoginAsync(dto, request.IpAddress);
    }
}
