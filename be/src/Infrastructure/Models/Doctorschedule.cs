using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Doctorschedule
{
    public int DoctorScheduleId { get; set; }

    public int DoctorId { get; set; }

    public int ShiftId { get; set; }

    public int? RoomId { get; set; }

    public int? EquipmentId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? DayOfWeek { get; set; }

    public string? RecurrenceRule { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Equipment? Equipment { get; set; }

    public virtual Room? Room { get; set; }

    public virtual Workshift Shift { get; set; } = null!;
}
