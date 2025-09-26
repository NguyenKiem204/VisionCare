using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Permissionrole
{
    public int PermissionId { get; set; }

    public int RoleId { get; set; }

    public DateTime? GrantedAt { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
