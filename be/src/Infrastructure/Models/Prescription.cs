using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public int EncounterId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Encounter Encounter { get; set; } = null!;

    public virtual ICollection<Prescriptionline> Prescriptionlines { get; set; } = new List<Prescriptionline>();
}
