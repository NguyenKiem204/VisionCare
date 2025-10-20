namespace VisionCare.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalAppointments { get; set; }
    public int CompletedAppointments { get; set; }
    public int CanceledAppointments { get; set; }
    public int NewPatients { get; set; }
    public decimal Revenue { get; set; }
}
