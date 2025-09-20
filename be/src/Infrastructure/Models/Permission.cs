using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string PermissionName { get; set; } = null!;

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
