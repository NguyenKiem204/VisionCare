using System;
using System.Collections.Generic;
using System.Net;

namespace VisionCare.Infrastructure.Models;

public partial class Auditlog
{
    public int AuditId { get; set; }

    public int? AccountId { get; set; }

    public string Action { get; set; } = null!;

    public string? Resource { get; set; }

    public int? ResourceId { get; set; }

    public DateTime? Timestamp { get; set; }

    public IPAddress? IpAddress { get; set; }

    public bool Success { get; set; }

    public string? Details { get; set; }

    public virtual Account? Account { get; set; }
}
