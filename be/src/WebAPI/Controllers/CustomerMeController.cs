using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VisionCare.Application.DTOs.AppointmentDto;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.WebAPI.Hubs;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/customer/me/appointments")]
[Authorize(Policy = "CustomerOnly")]
public class CustomerMeController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IHubContext<BookingHub> _hubContext;

    public CustomerMeController(
        IAppointmentService appointmentService,
        IHubContext<BookingHub> hubContext
    )
    {
        _appointmentService = appointmentService;
        _hubContext = hubContext;
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

    // Reschedule Workflow Endpoints
    [HttpPost("{id}/request-reschedule")]
    public async Task<ActionResult<AppointmentDto>> RequestRescheduleMyAppointment(
        int id,
        [FromBody] RequestRescheduleRequest request
    )
    {
        var customerId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.PatientId != customerId)
        {
            return Forbid();
        }

        try
        {
            var updated = await _appointmentService.RequestRescheduleAsync(
                id,
                request.ProposedDateTime,
                "Customer",
                request.Reason
            );

            // Send SignalR notification
            await _hubContext.Clients
                .Group($"appointment:{id}")
                .SendAsync("RescheduleRequested", new
                {
                    appointmentId = id,
                    proposedDateTime = request.ProposedDateTime,
                    requestedBy = "Customer",
                    reason = request.Reason
                });

            return Ok(ApiResponse<AppointmentDto>.Ok(updated));
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<AppointmentDto>.Fail(ex.Message));
        }
    }

    [HttpPut("{id}/approve-reschedule")]
    public async Task<ActionResult<AppointmentDto>> ApproveRescheduleMyAppointment(int id)
    {
        var customerId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.PatientId != customerId)
        {
            return Forbid();
        }

        try
        {
            var updated = await _appointmentService.ApproveRescheduleAsync(id, "Customer");

            // Send SignalR notification
            await _hubContext.Clients
                .Group($"appointment:{id}")
                .SendAsync("RescheduleApproved", new
                {
                    appointmentId = id,
                    newDateTime = updated.AppointmentDate
                });

            return Ok(ApiResponse<AppointmentDto>.Ok(updated));
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<AppointmentDto>.Fail(ex.Message));
        }
    }

    [HttpPut("{id}/reject-reschedule")]
    public async Task<ActionResult<AppointmentDto>> RejectRescheduleMyAppointment(
        int id,
        [FromBody] RejectRescheduleRequest? request = null
    )
    {
        var customerId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.PatientId != customerId)
        {
            return Forbid();
        }

        try
        {
            var updated = await _appointmentService.RejectRescheduleAsync(id, "Customer", request?.Reason);

            // Send SignalR notification
            await _hubContext.Clients
                .Group($"appointment:{id}")
                .SendAsync("RescheduleRejected", new
                {
                    appointmentId = id,
                    reason = request?.Reason
                });

            return Ok(ApiResponse<AppointmentDto>.Ok(updated));
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<AppointmentDto>.Fail(ex.Message));
        }
    }

    [HttpPost("{id}/counter-reschedule")]
    public async Task<ActionResult<AppointmentDto>> CounterRescheduleMyAppointment(
        int id,
        [FromBody] CounterRescheduleRequest request
    )
    {
        var customerId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.PatientId != customerId)
        {
            return Forbid();
        }

        try
        {
            var updated = await _appointmentService.CounterRescheduleAsync(
                id,
                request.ProposedDateTime,
                request.Reason
            );

            // Send SignalR notification
            await _hubContext.Clients
                .Group($"appointment:{id}")
                .SendAsync("RescheduleCounterProposed", new
                {
                    appointmentId = id,
                    proposedDateTime = request.ProposedDateTime,
                    requestedBy = "Customer",
                    reason = request.Reason
                });

            return Ok(ApiResponse<AppointmentDto>.Ok(updated));
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<AppointmentDto>.Fail(ex.Message));
        }
    }
}

