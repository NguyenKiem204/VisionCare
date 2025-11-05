using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Encounter
{
    public int EncounterId { get; set; }

    public int AppointmentId { get; set; }

    public int DoctorId { get; set; }

    public int CustomerId { get; set; }

    public string? Subjective { get; set; }

    public string? Objective { get; set; }

    public string? Assessment { get; set; }

    public string? Plan { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
