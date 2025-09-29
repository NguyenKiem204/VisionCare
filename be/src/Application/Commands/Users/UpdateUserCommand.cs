using AutoMapper;
using MediatR;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Commands.Users;

public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public int? RoleId { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingUser = await _userRepository.GetByIdAsync(request.Id);
        if (existingUser == null)
        {
            throw new ArgumentException($"User with ID {request.Id} not found.");
        }

        existingUser.Username = request.Username;
        existingUser.Email = request.Email;
        existingUser.Password = request.Password;
        existingUser.RoleId = request.RoleId;
        existingUser.LastModified = DateTime.UtcNow;

        await _userRepository.UpdateAsync(existingUser);

        return _mapper.Map<UserDto>(existingUser);
    }
}


