using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.ServiceTypeDto;

public class ServiceTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateServiceTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
}

public class UpdateServiceTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
}
