using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class FeedbackService
{
    public int FeedbackId { get; set; }

    public int? AppointmentId { get; set; }

    public string? FeedbackText { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public int? FeedbackRating { get; set; }

    public string? ResponseText { get; set; }

    public DateTime? ResponseDate { get; set; }

    public int? StaffId { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Staff? Staff { get; set; }
}
