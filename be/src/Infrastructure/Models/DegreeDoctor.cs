using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Degreedoctor
{
    public int DoctorId { get; set; }

    public int DegreeId { get; set; }

    public DateOnly? IssuedDate { get; set; }

    public string? IssuedBy { get; set; }

    public string? CertificateImage { get; set; }

    public string? Status { get; set; }

    public virtual Degree Degree { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;
}
