using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Benefits { get; set; }

    public string? Status { get; set; }

    public int? SpecializationId { get; set; }

    public virtual Imagesservice? Imagesservice { get; set; }

    public virtual ICollection<Servicesdetail> Servicesdetails { get; set; } = new List<Servicesdetail>();

    public virtual Specialization? Specialization { get; set; }
}
