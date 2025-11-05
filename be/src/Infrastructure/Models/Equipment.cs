using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string Name { get; set; } = null!;

    public string? Model { get; set; }

    public string? SerialNumber { get; set; }

    public string? Manufacturer { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public DateOnly? LastMaintenanceDate { get; set; }

    public string? Status { get; set; }

    public string? Location { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Doctorschedule> Doctorschedules { get; set; } = new List<Doctorschedule>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
