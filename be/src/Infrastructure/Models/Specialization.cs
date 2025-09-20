using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Specialization
{
    public int SpecializationId { get; set; }

    public string? SpecializationName { get; set; }

    public string? SpecializationStatus { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
