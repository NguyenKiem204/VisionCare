using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.ScheduleDto;

public class ScheduleDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public DateOnly ScheduleDate { get; set; }
    public int SlotId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateScheduleRequest
{
    public int DoctorId { get; set; }
    public DateOnly ScheduleDate { get; set; }
    public int SlotId { get; set; }
}

public class UpdateScheduleRequest
{
    public string? Status { get; set; }
}

public class ScheduleSearchRequest
{
    public int? DoctorId { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class AvailableSlotsRequest
{
    public int DoctorId { get; set; }
    public DateOnly ScheduleDate { get; set; }
    public int? ServiceTypeId { get; set; }
}
