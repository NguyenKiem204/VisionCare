using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string? ServiceDescription { get; set; }

    public string? ServiceIntroduce { get; set; }

    public string? ServiceBenefit { get; set; }

    public string? ServiceStatus { get; set; }

    public int? SpecializationId { get; set; }

    public virtual ImagesService? ImagesService { get; set; }

    public virtual ICollection<ServicesDetail> ServicesDetails { get; set; } = new List<ServicesDetail>();

    public virtual Specialization? Specialization { get; set; }
}
