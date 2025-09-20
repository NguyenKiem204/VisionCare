using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public int? AccountId { get; set; }

    public string DoctorName { get; set; } = null!;

    public int? ExperienceYears { get; set; }

    public int? SpecializationId { get; set; }

    public string? ProfileImage { get; set; }

    public double? Rating { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Address { get; set; }

    public string? DoctorStatus { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<CertificateDoctor> CertificateDoctors { get; set; } = new List<CertificateDoctor>();

    public virtual ICollection<DegreeDoctor> DegreeDoctors { get; set; } = new List<DegreeDoctor>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Specialization? Specialization { get; set; }
}
