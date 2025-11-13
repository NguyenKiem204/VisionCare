using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.Ehr;
using VisionCare.Application.Interfaces.Ehr;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/doctor/me/ehr")]
[Authorize(Policy = "DoctorOnly")]
public class DoctorEhrController : ControllerBase
{
    private readonly IEncounterService _encounterService;
    private readonly IPrescriptionService _prescriptionService;
    private readonly IOrderService _orderService;

    public DoctorEhrController(
        IEncounterService encounterService,
        IPrescriptionService prescriptionService,
        IOrderService orderService
    )
    {
        _encounterService = encounterService;
        _prescriptionService = prescriptionService;
        _orderService = orderService;
    }

    private int GetCurrentAccountId()
    {
        var idClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("account_id")?.Value
            ?? User.FindFirst("sub")?.Value;
        if (!int.TryParse(idClaim, out var accountId))
        {
            throw new UnauthorizedAccessException("Invalid or missing account id claim.");
        }
        return accountId;
    }

    [HttpPost("encounters")]
    public async Task<ActionResult<EncounterDto>> CreateEncounter(
        [FromBody] CreateEncounterRequest request,
        [FromQuery] int customerId
    )
    {
        var doctorId = GetCurrentAccountId();
        var created = await _encounterService.CreateAsync(doctorId, customerId, request);
        return Created(string.Empty, ApiResponse<EncounterDto>.Ok(created));
    }

    [HttpPut("encounters/{id}")]
    public async Task<ActionResult<EncounterDto>> UpdateEncounter(
        int id,
        [FromBody] UpdateEncounterRequest request
    )
    {
        var doctorId = GetCurrentAccountId();
        var updated = await _encounterService.UpdateAsync(id, doctorId, request);
        return Ok(ApiResponse<EncounterDto>.Ok(updated));
    }

    [HttpGet("encounters")]
    public async Task<ActionResult<IEnumerable<EncounterDto>>> GetMyEncounters(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to
    )
    {
        var doctorId = GetCurrentAccountId();
        var list = await _encounterService.GetByDoctorAndRangeAsync(doctorId, from, to);
        return Ok(ApiResponse<IEnumerable<EncounterDto>>.Ok(list));
    }

    [HttpGet("encounters/by-appointment/{appointmentId}")]
    public async Task<ActionResult<EncounterDto>> GetEncounterByAppointment(int appointmentId)
    {
        var doctorId = GetCurrentAccountId();
        var encounter = await _encounterService.GetByAppointmentIdAsync(appointmentId, doctorId);
        if (encounter == null)
            return NotFound(ApiResponse<EncounterDto>.Fail("Encounter not found"));
        return Ok(ApiResponse<EncounterDto>.Ok(encounter));
    }

    [HttpPost("prescriptions")]
    public async Task<ActionResult<PrescriptionDto>> CreatePrescription(
        [FromBody] CreatePrescriptionRequest request
    )
    {
        var doctorId = GetCurrentAccountId();
        var created = await _prescriptionService.CreateAsync(request, doctorId);
        return Created(string.Empty, ApiResponse<PrescriptionDto>.Ok(created));
    }

    [HttpGet("prescriptions/by-encounter/{encounterId}")]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptions(int encounterId)
    {
        var doctorId = GetCurrentAccountId();
        var list = await _prescriptionService.GetByEncounterAsync(encounterId, doctorId);
        return Ok(ApiResponse<IEnumerable<PrescriptionDto>>.Ok(list));
    }

    [HttpPost("orders")]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var doctorId = GetCurrentAccountId();
        var created = await _orderService.CreateAsync(request, doctorId);
        return Created(string.Empty, ApiResponse<OrderDto>.Ok(created));
    }

    [HttpGet("orders/by-encounter/{encounterId}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(int encounterId)
    {
        var doctorId = GetCurrentAccountId();
        var list = await _orderService.GetByEncounterAsync(encounterId, doctorId);
        return Ok(ApiResponse<IEnumerable<OrderDto>>.Ok(list));
    }

    [HttpPut("orders/{orderId}")]
    public async Task<ActionResult<OrderDto>> UpdateOrderResult(
        int orderId,
        [FromBody] UpdateOrderResultRequest request
    )
    {
        var doctorId = GetCurrentAccountId();
        var updated = await _orderService.UpdateResultAsync(orderId, doctorId, request);
        return Ok(ApiResponse<OrderDto>.Ok(updated));
    }
}
