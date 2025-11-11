using System;

namespace VisionCare.Application.DTOs;

public class WeeklyScheduleDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public string DayOfWeekName { get; set; } = string.Empty;
    public int SlotId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateWeeklyScheduleRequest
{
    public int DoctorId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int SlotId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateWeeklyScheduleRequest
{
    public DayOfWeek? DayOfWeek { get; set; }
    public int? SlotId { get; set; }
    public bool? IsActive { get; set; }
}

