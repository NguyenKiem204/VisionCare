namespace VisionCare.Application.DTOs.Ehr;

public class OrderDto
{
    public int Id { get; set; }
    public int EncounterId { get; set; }
    public string OrderType { get; set; } = string.Empty; // Test|Procedure
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Requested"; // Requested|InProgress|Completed|Canceled
    public string? ResultUrl { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateOrderRequest
{
    public int EncounterId { get; set; }
    public string OrderType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class UpdateOrderResultRequest
{
    public string? ResultUrl { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}


