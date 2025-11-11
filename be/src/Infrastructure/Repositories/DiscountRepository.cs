using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces;
using VisionCare.Infrastructure.Data;

namespace VisionCare.Infrastructure.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly VisionCareDbContext _context;

    public DiscountRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<Discount?> GetByIdAsync(int id)
    {
        var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == id);
        if (discount == null)
            return null;

        return new Discount
        {
            DiscountId = discount.DiscountId,
            DiscountPercent = discount.DiscountPercent,
        };
    }
}
