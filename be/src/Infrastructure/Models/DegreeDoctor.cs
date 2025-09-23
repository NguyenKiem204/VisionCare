using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class DegreeDoctor
{
    public int DegreeDoctorId { get; set; }

    public int? DoctorId { get; set; }

    public int? DegreeId { get; set; }

    public string? DegreeImage { get; set; }

    public DateTime? DateDegree { get; set; }

    public DateTime? DateChange { get; set; }

    public string? Status { get; set; }

    public string? IssuedBy { get; set; }

    public int? Version { get; set; }

    public virtual Degree? Degree { get; set; }

    public virtual Doctor? Doctor { get; set; }
}
