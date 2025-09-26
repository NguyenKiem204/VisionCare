using System;
using System.Collections.Generic;
using System.Net;

namespace VisionCare.Infrastructure.Models;

public partial class Refreshtoken
{
    public int TokenId { get; set; }

    public string TokenHash { get; set; } = null!;

    public int AccountId { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public IPAddress? CreatedByIp { get; set; }

    public virtual Account Account { get; set; } = null!;
}
