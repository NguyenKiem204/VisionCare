using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.FeedbackDto;

public class FeedbackDoctorDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public int Rating { get; set; }
    public string? FeedbackText { get; set; }
    public DateTime FeedbackDate { get; set; }
    public int? RespondedBy { get; set; }
    public string? ResponseText { get; set; }
    public DateTime? ResponseDate { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorName { get; set; }
    public DateTime? AppointmentDate { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateFeedbackDoctorRequest
{
    public int AppointmentId { get; set; }
    public int Rating { get; set; }
    public string? FeedbackText { get; set; }
}

public class UpdateFeedbackDoctorRequest
{
    public int Rating { get; set; }
    public string? FeedbackText { get; set; }
}

public class RespondToFeedbackRequest
{
    public string ResponseText { get; set; } = string.Empty;
}

public class FeedbackSearchRequest
{
    public int? DoctorId { get; set; }
    public int? PatientId { get; set; }
    public int? MinRating { get; set; }
    public int? MaxRating { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? HasResponse { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
