namespace VisionCare.Application.DTOs.Dashboard;

public class DoctorKpiDto
{
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int CompletedCount { get; set; }
    public decimal? Utilization { get; set; }
}
