using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Prescriptionline
{
    public int LineId { get; set; }

    public int PrescriptionId { get; set; }

    public string? DrugCode { get; set; }

    public string DrugName { get; set; } = null!;

    public string? Dosage { get; set; }

    public string? Frequency { get; set; }

    public string? Duration { get; set; }

    public string? Instructions { get; set; }

    public virtual Prescription Prescription { get; set; } = null!;
}
