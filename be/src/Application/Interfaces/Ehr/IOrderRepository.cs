namespace VisionCare.Application.Interfaces.Ehr;

public interface IOrderRepository
{
    Task<VisionCare.Domain.Entities.Order> AddAsync(VisionCare.Domain.Entities.Order order);
    Task<IEnumerable<VisionCare.Domain.Entities.Order>> GetByEncounterAsync(int encounterId);
    Task<VisionCare.Domain.Entities.Order?> GetByIdAsync(int orderId);
    Task UpdateAsync(VisionCare.Domain.Entities.Order order);
}
