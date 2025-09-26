using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Degree
{
    public int DegreeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Degreedoctor> Degreedoctors { get; set; } = new List<Degreedoctor>();
}
