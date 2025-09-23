using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class FollowUp
{
    public DateOnly? NextFollowUpDate { get; set; }

    public string? FollowUpDescription { get; set; }

    public string? Patientname { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
}
