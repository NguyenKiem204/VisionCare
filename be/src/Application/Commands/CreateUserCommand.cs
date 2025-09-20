using MediatR;
using AutoMapper;
using VisionCare.Application.DTOs;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Commands;

public class CreateUserCommand : IRequest<UserDto>
{
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
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

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            RoleId = request.RoleId,
            Created = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user);
        return _mapper.Map<UserDto>(createdUser);
    }
}
