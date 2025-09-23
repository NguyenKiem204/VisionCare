using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public string? AppointmentStatus { get; set; }

    public int? DoctorId { get; set; }

    public int? SlotId { get; set; }

    public int? ServiceDetailId { get; set; }

    public int? Discountid { get; set; }

    public decimal? Actualcost { get; set; }

    public int? PatientId { get; set; }

    public int? StaffId { get; set; }

    public virtual ICollection<Checkout> Checkouts { get; set; } = new List<Checkout>();

    public virtual Discount? Discount { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual ICollection<FeedbackDoctor> FeedbackDoctors { get; set; } = new List<FeedbackDoctor>();

    public virtual ICollection<FeedbackService> FeedbackServices { get; set; } = new List<FeedbackService>();

    public virtual Medicalhistory? Medicalhistory { get; set; }

    public virtual Customer? Patient { get; set; }

    public virtual ServicesDetail? ServiceDetail { get; set; }

    public virtual Slot? Slot { get; set; }

    public virtual Staff? Staff { get; set; }
}
