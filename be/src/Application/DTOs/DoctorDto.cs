using VisionCare.Application.DTOs.DoctorDto;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.DoctorDto;

public class DoctorDto
{
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int? ExperienceYears { get; set; }
    public int? SpecializationId { get; set; }
    public string? SpecializationName { get; set; }
    public string? ProfileImage { get; set; }
    public double? Rating { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
    public string? DoctorStatus { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
}

public class CreateDoctorRequest
{
    public int AccountId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int? ExperienceYears { get; set; }
    public int? SpecializationId { get; set; }
    public string? ProfileImage { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
}

public class UpdateDoctorRequest
{
    public string DoctorName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int? ExperienceYears { get; set; }
    public int? SpecializationId { get; set; }
    public string? ProfileImage { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
}

public class DoctorSearchRequest
{
    public string? Keyword { get; set; }
    public int? SpecializationId { get; set; }
    public double? MinRating { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
