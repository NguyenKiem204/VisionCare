using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Ehr;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using InfraOrder = VisionCare.Infrastructure.Models.Order;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories.Ehr;

public class OrderRepository : IOrderRepository
{
    private readonly VisionCareDbContext _db;

    public OrderRepository(VisionCareDbContext db)
    {
        _db = db;
    }

    public async Task<Order> AddAsync(Order order)
    {
        var model = ToInfra(order);
        _db.Orders.Add(model);
        await _db.SaveChangesAsync();
        return OrderMapper.ToDomain(model);
    }

    public async Task<IEnumerable<Order>> GetByEncounterAsync(int encounterId)
    {
        var list = await _db.Orders.Where(o => o.EncounterId == encounterId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
        return list.Select(OrderMapper.ToDomain).ToList();
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        var model = await _db.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        return model == null ? null : OrderMapper.ToDomain(model);
    }

    public async Task UpdateAsync(Order order)
    {
        var existing = await _db.Orders.FirstOrDefaultAsync(o => o.OrderId == order.Id);
        if (existing != null)
        {
            existing.Status = order.Status;
            existing.ResultUrl = order.ResultUrl;
            existing.Notes = order.Notes;
            existing.UpdatedAt = order.UpdatedAt;
            await _db.SaveChangesAsync();
        }
    }
    
    private static InfraOrder ToInfra(Order o) => OrderMapper.ToInfrastructure(o);
}


