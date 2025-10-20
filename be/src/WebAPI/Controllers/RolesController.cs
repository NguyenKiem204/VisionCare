using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.RoleDto;
using VisionCare.Application.Interfaces.Roles;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetRole(int id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return Ok(role);
    }

    /// <summary>
    /// Get role by name
    /// </summary>
    [HttpGet("name/{roleName}")]
    public async Task<ActionResult<RoleDto>> GetRoleByName(string roleName)
    {
        var role = await _roleService.GetRoleByNameAsync(roleName);
        if (role == null)
        {
            return NotFound();
        }
        return Ok(role);
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleRequest request)
    {
        var role = await _roleService.CreateRoleAsync(request);
        return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
    }

    /// <summary>
    /// Update role
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<RoleDto>> UpdateRole(int id, UpdateRoleRequest request)
    {
        var role = await _roleService.UpdateRoleAsync(id, request);
        return Ok(role);
    }

    /// <summary>
    /// Delete role
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRole(int id)
    {
        var result = await _roleService.DeleteRoleAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    /// Search roles
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<RoleDto>>> SearchRoles([FromQuery] string keyword)
    {
        var roles = await _roleService.SearchRolesAsync(keyword);
        return Ok(roles);
    }

    /// <summary>
    /// Check if role is in use
    /// </summary>
    [HttpGet("{id}/in-use")]
    public async Task<ActionResult<bool>> IsRoleInUse(int id)
    {
        var isInUse = await _roleService.IsRoleInUseAsync(id);
        return Ok(isInUse);
    }

    /// <summary>
    /// Get role statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<ActionResult<object>> GetRoleStatistics()
    {
        var totalCount = await _roleService.GetTotalRolesCountAsync();
        var usageStats = await _roleService.GetRoleUsageStatsAsync();

        return Ok(new
        {
            TotalCount = totalCount,
            UsageStats = usageStats
        });
    }
}
