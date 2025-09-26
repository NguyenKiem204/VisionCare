using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Otpservice
{
    public int OtpId { get; set; }

    public int AccountId { get; set; }

    public string OtpHash { get; set; } = null!;

    public string OtpType { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UsedAt { get; set; }

    public short? Attempts { get; set; }

    public virtual Account Account { get; set; } = null!;
}
