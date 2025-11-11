using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Customer
{
    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public int? RankId { get; set; }

    public string? Avatar { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    public virtual Customerrank? Rank { get; set; }
}
