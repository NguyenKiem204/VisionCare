using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Feedbackservice
{
    public int FeedbackId { get; set; }

    public int AppointmentId { get; set; }

    public short? Rating { get; set; }

    public string? FeedbackText { get; set; }

    public DateTime? FeedbackDate { get; set; }

    public string? ResponseText { get; set; }

    public DateTime? ResponseDate { get; set; }

    public int? RespondedBy { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Staff? RespondedByNavigation { get; set; }
}
