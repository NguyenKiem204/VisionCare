using AutoMapper;
using MediatR;
using VisionCare.Application.DTOs;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Commands;

public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
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
        // 1. Lấy user hiện tại
        var existingUser = await _userRepository.GetByIdAsync(request.Id);
        if (existingUser == null)
        {
            throw new ArgumentException($"User with ID {request.Id} not found.");
        }

        // 2. Cập nhật thông tin
        existingUser.Username = request.Username;
        existingUser.Email = request.Email;
        existingUser.PhoneNumber = request.PhoneNumber;
        existingUser.RoleId = request.RoleId;
        existingUser.LastModified = DateTime.UtcNow;

        // 3. Lưu vào database
        await _userRepository.UpdateAsync(existingUser);

        // 4. Trả về kết quả
        return _mapper.Map<UserDto>(existingUser);
    }
}
