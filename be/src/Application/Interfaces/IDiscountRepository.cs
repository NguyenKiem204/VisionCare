namespace VisionCare.Application.Interfaces;

public interface IDiscountRepository
{
    Task<Discount?> GetByIdAsync(int id);
}

public class Discount
{
    public int DiscountId { get; set; }
    public decimal DiscountPercent { get; set; }
    // Add other properties as needed
}
