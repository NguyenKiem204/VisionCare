namespace VisionCare.Application.DTOs.CustomerHistory;

public class CustomerBookingHistoryQuery
{
    public string? Status { get; set; }
    public bool UpcomingOnly { get; set; }
    public bool PastOnly { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "date";
    public bool SortDescending { get; set; } = true;
}

