using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.SpecializationDto;

public class SpecializationDto
{
    public int Id { get; set; }
    public string? SpecializationName { get; set; }
    public string? SpecializationStatus { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public int DoctorsCount { get; set; }
}

public class CreateSpecializationRequest
{
    public string SpecializationName { get; set; } = string.Empty;
}

public class UpdateSpecializationRequest
{
    public string SpecializationName { get; set; } = string.Empty;
    public string? SpecializationStatus { get; set; }
}

public class SpecializationSearchRequest
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
