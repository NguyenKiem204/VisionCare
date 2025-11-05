using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/doctor/me/analytics")]
[Authorize(Policy = "DoctorOnly")]
public class DoctorAnalyticsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public DoctorAnalyticsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
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

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var doctorId = GetCurrentAccountId();
        var result = await _appointmentService.SearchAppointmentsAsync(
            keyword: null,
            status: null,
            doctorId: doctorId,
            customerId: null,
            startDate: from,
            endDate: to,
            page: 1,
            pageSize: 1000,
            sortBy: null,
            desc: false
        );

        var items = result.items.ToList();
        var total = items.Count;
        var statusGroups = items
            .GroupBy(a => a.AppointmentStatus ?? "Unknown")
            .ToDictionary(g => g.Key, g => g.Count());
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var todayCount = items.Count(a =>
            a.AppointmentDate.HasValue && a.AppointmentDate.Value.Date == today.ToDateTime(TimeOnly.MinValue).Date
        );

        var summary = new
        {
            Total = total,
            Status = statusGroups,
            TodayAppointments = todayCount,
        };
        return Ok(ApiResponse<object>.Ok(summary));
    }
}
