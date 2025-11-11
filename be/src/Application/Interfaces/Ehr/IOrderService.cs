using VisionCare.Application.DTOs.Ehr;

namespace VisionCare.Application.Interfaces.Ehr;

public interface IOrderService
{
    Task<OrderDto> CreateAsync(CreateOrderRequest request, int doctorId);
    Task<IEnumerable<OrderDto>> GetByEncounterAsync(int encounterId, int doctorId);
    Task<OrderDto> UpdateResultAsync(int orderId, int doctorId, UpdateOrderResultRequest request);
}
