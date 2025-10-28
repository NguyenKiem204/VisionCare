namespace VisionCare.Application.DTOs.EquipmentDto;

public class EquipmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? Manufacturer { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public DateOnly? WarrantyExpiry { get; set; }
    public string Status { get; set; } = "Active";
    public string? Location { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? Notes { get; set; }
}

public class CreateEquipmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? Manufacturer { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public DateOnly? WarrantyExpiry { get; set; }
    public string Status { get; set; } = "Active";
    public string? Location { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? Notes { get; set; }
}

public class UpdateEquipmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public string? Manufacturer { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public DateOnly? WarrantyExpiry { get; set; }
    public string Status { get; set; } = "Active";
    public string? Location { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? Notes { get; set; }
}
