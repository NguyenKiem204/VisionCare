using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Claim
{
    public int ClaimId { get; set; }

    public int AccountId { get; set; }

    public string ClaimType { get; set; } = null!;

    public string ClaimValue { get; set; } = null!;

    public DateTime? ExpiresAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
