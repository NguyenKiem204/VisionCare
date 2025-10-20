using AutoMapper;
using VisionCare.Application.DTOs.RoleDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Roles;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Roles;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<RoleDto?> GetRoleByIdAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        return role != null ? _mapper.Map<RoleDto>(role) : null;
    }

    public async Task<RoleDto?> GetRoleByNameAsync(string roleName)
    {
        var role = await _roleRepository.GetByNameAsync(roleName);
        return role != null ? _mapper.Map<RoleDto>(role) : null;
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleRequest request)
    {
        // Check if role name already exists
        var existingRole = await _roleRepository.GetByNameAsync(request.RoleName);
        if (existingRole != null)
        {
            throw new ValidationException("Role with this name already exists.");
        }

        // Use AutoMapper to create entity from DTO
        var role = _mapper.Map<Role>(request);
        role.Created = DateTime.UtcNow;

        var createdRole = await _roleRepository.AddAsync(role);
        return _mapper.Map<RoleDto>(createdRole);
    }

    public async Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleRequest request)
    {
        var existingRole = await _roleRepository.GetByIdAsync(id);
        if (existingRole == null)
        {
            throw new NotFoundException($"Role with ID {id} not found.");
        }

        // Check if new name conflicts with existing roles
        if (request.RoleName != existingRole.RoleName)
        {
            var nameConflict = await _roleRepository.GetByNameAsync(request.RoleName);
            if (nameConflict != null && nameConflict.Id != id)
            {
                throw new ValidationException("Role with this name already exists.");
            }
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingRole);

        await _roleRepository.UpdateAsync(existingRole);
        return _mapper.Map<RoleDto>(existingRole);
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        var existingRole = await _roleRepository.GetByIdAsync(id);
        if (existingRole == null)
        {
            return false;
        }

        // Check if role is being used by users
        if (await IsRoleInUseAsync(id))
        {
            throw new ValidationException("Cannot delete role that is being used by users.");
        }

        await _roleRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<RoleDto>> SearchRolesAsync(string keyword)
    {
        var roles = await _roleRepository.SearchAsync(keyword);
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<bool> IsRoleInUseAsync(int roleId)
    {
        return await _roleRepository.IsInUseAsync(roleId);
    }

    public async Task<int> GetTotalRolesCountAsync()
    {
        return await _roleRepository.GetTotalCountAsync();
    }

    public async Task<Dictionary<string, int>> GetRoleUsageStatsAsync()
    {
        return await _roleRepository.GetUsageStatsAsync();
    }
}
