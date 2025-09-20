using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class OtpService
{
    public int OtpId { get; set; }

    public int? AccountId { get; set; }

    public string? Otp { get; set; }

    public string? CreatedOtpTime { get; set; }

    public string? OtpExpiryDate { get; set; }

    public virtual Account? Account { get; set; }
}
