using VisionCare.Application.DTOs.Ehr;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.Ehr;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Ehr;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEncounterRepository _encounterRepository;

    public OrderService(IOrderRepository orderRepository, IEncounterRepository encounterRepository)
    {
        _orderRepository = orderRepository;
        _encounterRepository = encounterRepository;
    }

    public async Task<OrderDto> CreateAsync(CreateOrderRequest request, int doctorId)
    {
        var e = await _encounterRepository.GetByIdAsync(request.EncounterId);
        if (e == null)
            throw new NotFoundException("Encounter not found");
        if (e.DoctorId != doctorId)
            throw new UnauthorizedAccessException("Cannot create order for another doctor's encounter");

        var o = new Order
        {
            EncounterId = request.EncounterId,
            OrderType = request.OrderType,
            Name = request.Name,
            Status = "Requested",
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
        };
        var created = await _orderRepository.AddAsync(o);
        return Map(created);
    }

    public async Task<IEnumerable<OrderDto>> GetByEncounterAsync(int encounterId, int doctorId)
    {
        var e = await _encounterRepository.GetByIdAsync(encounterId);
        if (e == null)
            throw new NotFoundException("Encounter not found");
        if (e.DoctorId != doctorId)
            throw new UnauthorizedAccessException("Cannot view orders of another doctor's encounter");

        var list = await _orderRepository.GetByEncounterAsync(encounterId);
        return list.Select(Map).ToList();
    }

    public async Task<OrderDto> UpdateResultAsync(
        int orderId,
        int doctorId,
        UpdateOrderResultRequest request
    )
    {
        var o = await _orderRepository.GetByIdAsync(orderId);
        if (o == null)
            throw new NotFoundException("Order not found");
        var e = await _encounterRepository.GetByIdAsync(o.EncounterId);
        if (e == null || e.DoctorId != doctorId)
            throw new UnauthorizedAccessException("Cannot update order for another doctor's encounter");

        if (request.ResultUrl != null) o.ResultUrl = request.ResultUrl;
        if (!string.IsNullOrWhiteSpace(request.Status)) o.Status = request.Status!;
        if (request.Notes != null) o.Notes = request.Notes;
        o.UpdatedAt = DateTime.UtcNow;
        await _orderRepository.UpdateAsync(o);
        return Map(o);
    }

    private static OrderDto Map(Order o)
    {
        return new OrderDto
        {
            Id = o.Id,
            EncounterId = o.EncounterId,
            OrderType = o.OrderType,
            Name = o.Name,
            Status = o.Status,
            ResultUrl = o.ResultUrl,
            Notes = o.Notes,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt,
        };
    }
}


