using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Certificate
{
    public int CertificateId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Certificatedoctor> Certificatedoctors { get; set; } = new List<Certificatedoctor>();
}
