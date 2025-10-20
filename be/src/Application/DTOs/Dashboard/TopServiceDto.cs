namespace VisionCare.Application.DTOs.Dashboard;

public class TopServiceDto
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public int CompletedCount { get; set; }
}
