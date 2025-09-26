using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Certificatedoctor
{
    public int DoctorId { get; set; }

    public int CertificateId { get; set; }

    public DateOnly? IssuedDate { get; set; }

    public string? IssuedBy { get; set; }

    public string? CertificateImage { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? Status { get; set; }

    public virtual Certificate Certificate { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;
}
