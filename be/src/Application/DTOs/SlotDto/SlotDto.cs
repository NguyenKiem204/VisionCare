using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.SlotDto;

public class SlotDto
{
    public int Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int ServiceTypeId { get; set; }
    public string? ServiceTypeName { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateSlotRequest
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int ServiceTypeId { get; set; }
}

public class UpdateSlotRequest
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

public class SlotSearchRequest
{
    public int? ServiceTypeId { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
