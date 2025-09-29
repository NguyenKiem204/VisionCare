using AutoMapper;
using MediatR;
using VisionCare.Application.DTOs.User;
using VisionCare.Application.Interfaces;

namespace VisionCare.Application.Queries.Users;

public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>> { }

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
}


