using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Doctorabsence
{
    public int AbsenceId { get; set; }

    public int DoctorId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? AbsenceType { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public bool? IsResolved { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;
}
