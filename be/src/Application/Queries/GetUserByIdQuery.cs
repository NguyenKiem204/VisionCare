using MediatR;
using AutoMapper;
using VisionCare.Application.DTOs;
using VisionCare.Application.Interfaces;

namespace VisionCare.Application.Queries;

public class GetUserByIdQuery : IRequest<UserDto?>
{
    public int Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Lấy user từ database
        var user = await _userRepository.GetByIdAsync(request.Id);
        
        // 2. Nếu không tìm thấy, trả về null
        if (user == null)
        {
            return null;
        }

        // 3. Map sang DTO và trả về
        return _mapper.Map<UserDto>(user);
    }
}
