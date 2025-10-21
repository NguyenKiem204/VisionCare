using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.ServiceDetailDto;

public class ServiceDetailDto
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public int ServiceTypeId { get; set; }
    public decimal Cost { get; set; }
    public string? ServiceName { get; set; }
    public string? ServiceTypeName { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateServiceDetailRequest
{
    public int ServiceId { get; set; }
    public int ServiceTypeId { get; set; }
    public decimal Cost { get; set; }
}

public class UpdateServiceDetailRequest
{
    public decimal Cost { get; set; }
}

public class ServiceDetailSearchRequest
{
    public int? ServiceId { get; set; }
    public int? ServiceTypeId { get; set; }
    public decimal? MinCost { get; set; }
    public decimal? MaxCost { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
