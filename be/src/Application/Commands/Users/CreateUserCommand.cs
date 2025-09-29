using AutoMapper;
using MediatR;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Commands.Users;

public class CreateUserCommand : IRequest<UserDto>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Password { get; set; }
    public int? RoleId { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            RoleId = request.RoleId,
            CreatedDate = DateTime.UtcNow,
        };

        var createdUser = await _userRepository.AddAsync(user);
        return _mapper.Map<UserDto>(createdUser);
    }
}


