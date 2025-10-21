using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class FeedbackService : BaseEntity
{
    public int AppointmentId { get; set; }
    public int Rating { get; set; }
    public string? FeedbackText { get; set; }
    public DateTime FeedbackDate { get; set; }
    public int? RespondedBy { get; set; }
    public string? ResponseText { get; set; }
    public DateTime? ResponseDate { get; set; }

    // Navigation properties
    public Appointment Appointment { get; set; } = null!;
    public User? RespondedByUser { get; set; }

    // Domain methods
    public void UpdateRating(int newRating)
    {
        if (newRating < 1 || newRating > 5)
            throw new ArgumentException("Rating must be between 1 and 5");

        Rating = newRating;
        LastModified = DateTime.UtcNow;
    }

    public void UpdateFeedback(string feedbackText)
    {
        FeedbackText = feedbackText;
        LastModified = DateTime.UtcNow;
    }

    public void AddResponse(int respondedBy, string responseText)
    {
        RespondedBy = respondedBy;
        ResponseText = responseText;
        ResponseDate = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;
    }

    public bool IsRated()
    {
        return Rating > 0;
    }

    public bool HasResponse()
    {
        return !string.IsNullOrEmpty(ResponseText);
    }

    public bool IsValidRating()
    {
        return Rating >= 1 && Rating <= 5;
    }
}
