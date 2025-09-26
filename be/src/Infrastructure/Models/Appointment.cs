using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public int ServiceDetailId { get; set; }

    public int? DiscountId { get; set; }

    public DateTime AppointmentDatetime { get; set; }

    public string? Status { get; set; }

    public decimal? ActualCost { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public virtual ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual ICollection<Feedbackdoctor> Feedbackdoctors { get; set; } = new List<Feedbackdoctor>();

    public virtual ICollection<Feedbackservice> Feedbackservices { get; set; } = new List<Feedbackservice>();

    public virtual ICollection<Followup> Followups { get; set; } = new List<Followup>();

    public virtual ICollection<Medicalhistory> Medicalhistories { get; set; } = new List<Medicalhistory>();

    public virtual Customer Patient { get; set; } = null!;

    public virtual Servicesdetail ServiceDetail { get; set; } = null!;
}
