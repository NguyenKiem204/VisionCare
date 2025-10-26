using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Equipment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? Manufacturer { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public DateOnly? LastMaintenanceDate { get; set; }
    public string Status { get; set; } = "Active"; // Active, Inactive, Maintenance, Broken
    public string? Location { get; set; }
    public string? Notes { get; set; }

    // Domain methods
    public void UpdateName(string newName)
    {
        Name = newName;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateStatus(string status)
    {
        Status = status;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateLocation(string? location)
    {
        Location = location;
        LastModified = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = "Active";
        LastModified = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = "Inactive";
        LastModified = DateTime.UtcNow;
    }

    public void SetMaintenance()
    {
        Status = "Maintenance";
        LastModified = DateTime.UtcNow;
    }

    public void SetBroken()
    {
        Status = "Broken";
        LastModified = DateTime.UtcNow;
    }
}
