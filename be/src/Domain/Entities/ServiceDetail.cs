using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class ServiceDetail : BaseEntity
{
    public int ServiceId { get; set; }
    public int ServiceTypeId { get; set; }
    public decimal Cost { get; set; }

    // Navigation properties
    public Service Service { get; set; } = null!;
    public ServiceType ServiceType { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    // Domain methods
    public void UpdateCost(decimal newCost)
    {
        if (newCost < 0)
            throw new ArgumentException("Cost cannot be negative");

        Cost = newCost;
        LastModified = DateTime.UtcNow;
    }

    public decimal CalculateDiscountedCost(decimal discountPercent)
    {
        if (discountPercent < 0 || discountPercent > 100)
            throw new ArgumentException("Discount percent must be between 0 and 100");

        return Cost * (1 - discountPercent / 100);
    }
}
