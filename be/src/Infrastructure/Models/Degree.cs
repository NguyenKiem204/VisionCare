using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Degree
{
    public int DegreeId { get; set; }

    public string? DegreeName { get; set; }

    public virtual ICollection<DegreeDoctor> DegreeDoctors { get; set; } = new List<DegreeDoctor>();
}
