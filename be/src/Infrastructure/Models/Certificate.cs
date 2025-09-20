using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public string? CertificateName { get; set; }

    public virtual ICollection<CertificateDoctor> CertificateDoctors { get; set; } = new List<CertificateDoctor>();
}
