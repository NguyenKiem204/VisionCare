using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.RoleDto;

public class RoleDto
{
    public int Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public int UsersCount { get; set; }
}

public class CreateRoleRequest
{
    public string RoleName { get; set; } = string.Empty;
}

public class UpdateRoleRequest
{
    public string RoleName { get; set; } = string.Empty;
}

public class RoleSearchRequest
{
    public string? Keyword { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
