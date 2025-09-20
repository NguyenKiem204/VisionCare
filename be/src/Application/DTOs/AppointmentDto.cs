namespace VisionCare.Application.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public string? AppointmentStatus { get; set; }
    public string? DoctorName { get; set; }
    public string? PatientName { get; set; }
    public decimal? ActualCost { get; set; }
}
