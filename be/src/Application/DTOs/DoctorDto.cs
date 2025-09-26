namespace VisionCare.Application.DTOs;

public class DoctorDto
{
    public int Id { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int? ExperienceYears { get; set; }
    public string? SpecializationName { get; set; }
    public double? Rating { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
}
