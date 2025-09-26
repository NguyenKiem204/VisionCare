using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? RoleDescription { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Permissionrole> Permissionroles { get; set; } = new List<Permissionrole>();
}
