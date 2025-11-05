using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

/// <summary>
/// Room entity - represents a physical room/location in the clinic
/// </summary>
public class Room : BaseEntity
{
    public string RoomName { get; set; } = string.Empty;
    public string? RoomCode { get; set; }
    public int Capacity { get; set; } = 1;
    public string Status { get; set; } = "Active"; // Active, Maintenance, Inactive
    public string? Location { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

    // Domain methods
    public void UpdateName(string roomName)
    {
        if (string.IsNullOrWhiteSpace(roomName))
            throw new ArgumentException("Room name cannot be empty");

        RoomName = roomName;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateStatus(string status)
    {
        if (!IsValidStatus(status))
            throw new ArgumentException($"Invalid status: {status}");

        Status = status;
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

    public bool IsActive()
    {
        return Status == "Active";
    }

    public bool IsAvailable()
    {
        return IsActive() && Status != "Maintenance";
    }

    private static bool IsValidStatus(string status)
    {
        return status is "Active" or "Maintenance" or "Inactive";
    }
}
