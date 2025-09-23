using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class ContentStory
{
    public string PatientName { get; set; } = null!;

    public string? ImagePatient { get; set; }

    public string? ContentStories { get; set; }
}
