namespace VisionCare.Application.DTOs.Dashboard;

public class TimeSeriesPointDto
{
    public string Label { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal? Amount { get; set; }
}
