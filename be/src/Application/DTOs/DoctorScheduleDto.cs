namespace VisionCare.Application.DTOs.DoctorScheduleDto;

public class DoctorScheduleDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public int ShiftId { get; set; }
    public string? ShiftName { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public int? RoomId { get; set; }
    public string? RoomName { get; set; }
    public int? EquipmentId { get; set; }
    public string? EquipmentName { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? DayOfWeek { get; set; } // 1=Monday, 2=Tuesday, ..., 7=Sunday
    public string? DayOfWeekName { get; set; }
    public string RecurrenceRule { get; set; } = "WEEKLY";
    public bool IsActive { get; set; } = true;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateDoctorScheduleRequest
{
    public int DoctorId { get; set; }
    public int ShiftId { get; set; }
    public int? RoomId { get; set; }
    public int? EquipmentId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? DayOfWeek { get; set; } // 1=Monday, 2=Tuesday, ..., 7=Sunday
    public string RecurrenceRule { get; set; } = "WEEKLY";
    public bool IsActive { get; set; } = true;
}

public class UpdateDoctorScheduleRequest
{
    public int ShiftId { get; set; }
    public int? RoomId { get; set; }
    public int? EquipmentId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? DayOfWeek { get; set; }
    public string RecurrenceRule { get; set; } = "WEEKLY";
    public bool IsActive { get; set; } = true;
}

