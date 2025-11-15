using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

/// <summary>
/// DoctorSchedule entity - flexible scheduling with recurrence rules
/// </summary>
public class DoctorSchedule : BaseEntity
{
    public int DoctorId { get; set; }
    public int ShiftId { get; set; }
    public int? RoomId { get; set; }
    public int? EquipmentId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? DayOfWeek { get; set; } // 1=Monday, 2=Tuesday, ..., 7=Sunday, NULL = all days
    public string RecurrenceRule { get; set; } = "WEEKLY"; // DAILY, WEEKLY, MONTHLY, CUSTOM
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Doctor Doctor { get; set; } = null!;
    public WorkShift Shift { get; set; } = null!;
    public Room? Room { get; set; }
    public Equipment? Equipment { get; set; }

    // Domain methods
    public void Activate()
    {
        IsActive = true;
        LastModified = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateRecurrence(string recurrenceRule, int? dayOfWeek = null)
    {
        if (!IsValidRecurrenceRule(recurrenceRule))
            throw new ArgumentException($"Invalid recurrence rule: {recurrenceRule}");

        RecurrenceRule = recurrenceRule;
        DayOfWeek = dayOfWeek;
        LastModified = DateTime.UtcNow;
    }

    public void SetEndDate(DateOnly? endDate)
    {
        if (endDate.HasValue && endDate.Value < StartDate)
            throw new ArgumentException("End date cannot be before start date");

        EndDate = endDate;
        LastModified = DateTime.UtcNow;
    }

    public bool IsValid()
    {
        if (!IsActive) return false;
        // Check if schedule has expired (EndDate is in the past)
        if (EndDate.HasValue && EndDate.Value < DateOnly.FromDateTime(DateTime.Today))
            return false;

        // Schedule is valid if it's active and not expired
        // StartDate can be in the future - that's OK, AppliesToDate will handle it
        return true;
    }

    public bool AppliesToDate(DateOnly date)
    {
        if (!IsValid()) return false;
        if (date < StartDate) return false;
        if (EndDate.HasValue && date > EndDate.Value) return false;

        return RecurrenceRule switch
        {
            "DAILY" => true,
            "WEEKLY" => DayOfWeek == null || GetDayOfWeek(date) == DayOfWeek,
            "MONTHLY" => date.Day == StartDate.Day,
            "CUSTOM" => DayOfWeek == null || GetDayOfWeek(date) == DayOfWeek,
            _ => false
        };
    }

    private static int GetDayOfWeek(DateOnly date)
    {
        // Convert to ISO 8601 day of week (1=Monday, 7=Sunday)
        var day = date.DayOfWeek;
        return day == System.DayOfWeek.Sunday ? 7 : (int)day;
    }

    private static bool IsValidRecurrenceRule(string rule)
    {
        return rule is "DAILY" or "WEEKLY" or "MONTHLY" or "CUSTOM";
    }
}

