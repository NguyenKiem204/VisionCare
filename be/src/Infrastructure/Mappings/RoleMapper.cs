using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class RoleMapper
{
    public static Role ToDomain(VisionCare.Infrastructure.Models.Role model)
    {
        return new Role
        {
            Id = model.RoleId,
            RoleName = model.RoleName,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };
    }

    public static VisionCare.Infrastructure.Models.Role ToInfrastructure(Role domain)
    {
        return new VisionCare.Infrastructure.Models.Role
        {
            RoleId = domain.Id,
            RoleName = domain.RoleName,
        };
    }
}
