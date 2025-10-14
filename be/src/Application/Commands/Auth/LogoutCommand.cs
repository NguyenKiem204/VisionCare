using MediatR;
using VisionCare.Application.Interfaces.Auth;

namespace VisionCare.Application.Commands.Auth;

public class LogoutCommand : IRequest<Unit>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _authService.RevokeAsync(request.RefreshToken, request.IpAddress);
        return Unit.Value;
    }
}


