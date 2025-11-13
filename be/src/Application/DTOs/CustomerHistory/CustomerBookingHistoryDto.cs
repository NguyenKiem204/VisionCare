namespace VisionCare.Application.DTOs.CustomerHistory;

public class CustomerBookingHistoryDto
{
    public int AppointmentId { get; set; }
    public string AppointmentCode { get; set; } = string.Empty;
    public DateTime? AppointmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public decimal? TotalAmount { get; set; }
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string? DoctorAvatar { get; set; }
    public string? DoctorSpecialization { get; set; }
    public int ServiceId { get; set; }
    public int ServiceDetailId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceTypeName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

