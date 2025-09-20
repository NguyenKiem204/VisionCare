using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Doctor : BaseEntity
{
    public int? AccountId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int? ExperienceYears { get; set; }
    public int? SpecializationId { get; set; }
    public string? ProfileImage { get; set; }
    public double? Rating { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
    public string? DoctorStatus { get; set; }

    // Navigation properties
    public User? Account { get; set; }
    public Specialization? Specialization { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
