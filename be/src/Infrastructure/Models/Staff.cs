using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Staff
{
    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Gender { get; set; }

    public string? Avatar { get; set; }

    public DateOnly? HiredDate { get; set; }

    public decimal? Salary { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Feedbackdoctor> Feedbackdoctors { get; set; } = new List<Feedbackdoctor>();

    public virtual ICollection<Feedbackservice> Feedbackservices { get; set; } = new List<Feedbackservice>();
}
