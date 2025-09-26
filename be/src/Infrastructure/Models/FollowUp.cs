using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Followup
{
    public int FollowUpId { get; set; }

    public int AppointmentId { get; set; }

    public DateOnly? NextAppointmentDate { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;
}
