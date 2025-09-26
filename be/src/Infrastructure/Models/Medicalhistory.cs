using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Medicalhistory
{
    public int MedicalHistoryId { get; set; }

    public int AppointmentId { get; set; }

    public string? Diagnosis { get; set; }

    public string? Symptoms { get; set; }

    public string? Treatment { get; set; }

    public string? Prescription { get; set; }

    public decimal? VisionLeft { get; set; }

    public decimal? VisionRight { get; set; }

    public string? AdditionalTests { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
}
