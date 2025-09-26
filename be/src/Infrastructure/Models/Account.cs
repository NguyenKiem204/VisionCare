using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public bool? EmailConfirmed { get; set; }

    public string? EmailConfirmationToken { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public short? AccessFailedCount { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? PasswordResetExpires { get; set; }

    public DateTime? LastPasswordChange { get; set; }

    public string? GoogleId { get; set; }

    public string? FacebookId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? Status { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Auditlog> Auditlogs { get; set; } = new List<Auditlog>();

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();

    public virtual ICollection<Commentblog> Commentblogs { get; set; } = new List<Commentblog>();

    public virtual Customer? Customer { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual ICollection<Otpservice> Otpservices { get; set; } = new List<Otpservice>();

    public virtual ICollection<Refreshtoken> Refreshtokens { get; set; } = new List<Refreshtoken>();

    public virtual Role Role { get; set; } = null!;

    public virtual Staff? Staff { get; set; }
}
