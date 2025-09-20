using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Customer
{
    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Address { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public int? Rankid { get; set; }

    public string? ImageProfileUser { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Customerrank? Rank { get; set; }
}
