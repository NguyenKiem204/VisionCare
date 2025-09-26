using MediatR;
using VisionCare.Application.Interfaces;

namespace VisionCare.Application.Commands;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByIdAsync(request.Id);
        if (existingUser == null)
        {
            return false;
        }
        await _userRepository.DeleteAsync(request.Id);

        return true;
    }
}
