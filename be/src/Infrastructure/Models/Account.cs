using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? RoleId { get; set; }

    public string? GoogleId { get; set; }

    public string? FacebookId { get; set; }

    public string? FirstConfirm { get; set; }

    public string? StatusAccount { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Commentblog> Commentblogs { get; set; } = new List<Commentblog>();

    public virtual Customer? Customer { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual ICollection<OtpService> OtpServices { get; set; } = new List<OtpService>();

    public virtual Role? Role { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual ICollection<TokenUser> TokenUsers { get; set; } = new List<TokenUser>();
}
