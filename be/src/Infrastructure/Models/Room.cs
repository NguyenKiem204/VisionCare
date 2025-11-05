using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    public string? RoomCode { get; set; }

    public int? Capacity { get; set; }

    public string? Status { get; set; }

    public string? Location { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Doctorschedule> Doctorschedules { get; set; } = new List<Doctorschedule>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
