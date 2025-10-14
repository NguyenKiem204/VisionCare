using MediatR;
using VisionCare.Application.DTOs.Auth;
using VisionCare.Application.Interfaces.Auth;

namespace VisionCare.Application.Commands.Auth;

public class RefreshTokenCommand : IRequest<TokenResponse>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenResponse>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<TokenResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        return await _authService.RefreshAsync(request.RefreshToken, request.IpAddress);
    }
}
