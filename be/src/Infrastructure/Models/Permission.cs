using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string PermissionName { get; set; } = null!;

    public string Resource { get; set; } = null!;

    public string Action { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Permissionrole> Permissionroles { get; set; } = new List<Permissionrole>();
}
