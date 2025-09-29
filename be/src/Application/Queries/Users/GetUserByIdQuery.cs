using AutoMapper;
using MediatR;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;

namespace VisionCare.Application.Queries.Users;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.Id} not found.");
        }
        return _mapper.Map<UserDto>(user);
    }
}


