using System;
using System.Collections.Generic;

namespace VisionCare.Infrastructure.Models;

public partial class Machine
{
    public int MachineId { get; set; }

    public string? MachineName { get; set; }

    public string? MachineDescription { get; set; }

    public string? MachineImg { get; set; }
}
