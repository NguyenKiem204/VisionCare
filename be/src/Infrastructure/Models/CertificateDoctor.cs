using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class CertificateDoctor
{
    public int CertificateId { get; set; }

    public int DoctorId { get; set; }

    public DateTime? DateCertificate { get; set; }

    public DateTime? DateChange { get; set; }

    public string? Status { get; set; }

    public string? IssuedBy { get; set; }

    public string? CertificateImage { get; set; }

    public virtual Certificate Certificate { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;
}
