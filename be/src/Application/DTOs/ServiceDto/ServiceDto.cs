using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.ServiceDto;

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Benefits { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? SpecializationId { get; set; }
    public string? SpecializationName { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinDuration { get; set; }
    public int? MaxDuration { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public ICollection<ServiceDetailDto.ServiceDetailDto> ServiceDetails { get; set; } =
        new List<ServiceDetailDto.ServiceDetailDto>();
}

public class CreateServiceRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Benefits { get; set; }
    public int? SpecializationId { get; set; }
}

public class UpdateServiceRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Benefits { get; set; }
    public int? SpecializationId { get; set; }
}

public class ServiceSearchRequest
{
    public string? Keyword { get; set; }
    public int? SpecializationId { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
