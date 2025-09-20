using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int? AccountId { get; set; }

    public string? AdminFullname { get; set; }

    public string? AdminAddress { get; set; }

    public DateOnly? AdminDob { get; set; }

    public string? AdminGender { get; set; }

    public string? ImageProfileAdmin { get; set; }

    public DateTime? AdminHiredDate { get; set; }

    public decimal? AdminSalary { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<FeedbackDoctor> FeedbackDoctors { get; set; } = new List<FeedbackDoctor>();

    public virtual ICollection<FeedbackService> FeedbackServices { get; set; } = new List<FeedbackService>();
}
