using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Doctor
{
    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public short? ExperienceYears { get; set; }

    public int SpecializationId { get; set; }

    public string? Avatar { get; set; }

    public decimal? Rating { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Address { get; set; }

    public string? Status { get; set; }

    public string? Biography { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Certificatedoctor> Certificatedoctors { get; set; } = new List<Certificatedoctor>();

    public virtual ICollection<Degreedoctor> Degreedoctors { get; set; } = new List<Degreedoctor>();

    public virtual ICollection<Doctorabsence> Doctorabsences { get; set; } = new List<Doctorabsence>();

    public virtual ICollection<Doctorschedule> Doctorschedules { get; set; } = new List<Doctorschedule>();

    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Specialization Specialization { get; set; } = null!;

    public virtual ICollection<Weeklyschedule> Weeklyschedules { get; set; } = new List<Weeklyschedule>();
}
