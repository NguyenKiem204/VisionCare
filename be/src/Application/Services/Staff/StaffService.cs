using AutoMapper;
using VisionCare.Application.DTOs.StaffDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Staff;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Staff;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public StaffService(
        IStaffRepository staffRepository,
        IUserRepository userRepository,
        IMapper mapper
    )
    {
        _staffRepository = staffRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StaffDto>> GetAllStaffAsync()
    {
        var staff = await _staffRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<StaffDto>>(staff);
    }

    public async Task<StaffDto?> GetStaffByIdAsync(int id)
    {
        var staff = await _staffRepository.GetByIdAsync(id);
        return staff != null ? _mapper.Map<StaffDto>(staff) : null;
    }

    public async Task<StaffDto?> GetStaffByAccountIdAsync(int accountId)
    {
        var staff = await _staffRepository.GetByAccountIdAsync(accountId);
        return staff != null ? _mapper.Map<StaffDto>(staff) : null;
    }

    public async Task<StaffDto> CreateStaffAsync(CreateStaffRequest request)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(request.AccountId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.AccountId} not found.");
        }

        // Check if user already has a staff profile
        if (user.Staff != null)
        {
            throw new ValidationException("User already has a staff profile.");
        }

        // Use AutoMapper to create entity from DTO
        var staff = _mapper.Map<VisionCare.Domain.Entities.Staff>(request);
        staff.Created = DateTime.UtcNow;

        var createdStaff = await _staffRepository.AddAsync(staff);
        return _mapper.Map<StaffDto>(createdStaff);
    }

    public async Task<StaffDto> UpdateStaffAsync(int id, UpdateStaffRequest request)
    {
        var existingStaff = await _staffRepository.GetByIdAsync(id);
        if (existingStaff == null)
        {
            throw new NotFoundException($"Staff with ID {id} not found.");
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingStaff);

        await _staffRepository.UpdateAsync(existingStaff);
        return _mapper.Map<StaffDto>(existingStaff);
    }

    public async Task<bool> DeleteStaffAsync(int id)
    {
        var existingStaff = await _staffRepository.GetByIdAsync(id);
        if (existingStaff == null)
        {
            return false;
        }

        await _staffRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<StaffDto>> SearchStaffAsync(string keyword, string? gender)
    {
        var staff = await _staffRepository.SearchAsync(keyword, gender);
        return _mapper.Map<IEnumerable<StaffDto>>(staff);
    }

    public async Task<IEnumerable<StaffDto>> GetStaffByGenderAsync(string gender)
    {
        var staff = await _staffRepository.GetByGenderAsync(gender);
        return _mapper.Map<IEnumerable<StaffDto>>(staff);
    }

    public async Task<StaffDto> UpdateStaffProfileAsync(
        int staffId,
        UpdateStaffProfileRequest request
    )
    {
        var staff = await _staffRepository.GetByIdAsync(staffId);
        if (staff == null)
        {
            throw new NotFoundException($"Staff with ID {staffId} not found.");
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, staff);

        await _staffRepository.UpdateAsync(staff);
        return _mapper.Map<StaffDto>(staff);
    }

    public async Task<int> GetTotalStaffCountAsync()
    {
        return await _staffRepository.GetTotalCountAsync();
    }

    public async Task<Dictionary<string, int>> GetStaffByGenderStatsAsync()
    {
        return await _staffRepository.GetGenderStatsAsync();
    }
}
