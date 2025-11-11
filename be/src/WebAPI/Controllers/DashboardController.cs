using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.Interfaces.Reporting;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("admin/summary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAdminSummary(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct
    )
    {
        var data = await _dashboardService.GetAdminSummaryAsync(
            from.ToUniversalTime(),
            to.ToUniversalTime(),
            ct
        );
        return Ok(ApiResponse.Success(data));
    }

    [HttpGet("admin/appointments/series")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAdminAppointmentSeries(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] string bucket = "day",
        CancellationToken ct = default
    )
    {
        var data = await _dashboardService.GetAppointmentSeriesAsync(
            from.ToUniversalTime(),
            to.ToUniversalTime(),
            bucket,
            null,
            ct
        );
        return Ok(ApiResponse.Success(data));
    }

    [HttpGet("admin/services/top")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTopServices(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] int top = 5,
        CancellationToken ct = default
    )
    {
        var data = await _dashboardService.GetTopServicesAsync(
            from.ToUniversalTime(),
            to.ToUniversalTime(),
            top,
            ct
        );
        return Ok(ApiResponse.Success(data));
    }

    [HttpGet("admin/doctors/kpis")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDoctorKpis(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct
    )
    {
        var data = await _dashboardService.GetDoctorKpisAsync(
            from.ToUniversalTime(),
            to.ToUniversalTime(),
            ct
        );
        return Ok(ApiResponse.Success(data));
    }

    [HttpGet("doctor/summary")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorSummary(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct
    )
    {
        var doctorIdStr = User.FindFirst("doctorId")?.Value;
        if (!int.TryParse(doctorIdStr, out var doctorId))
            return Forbid();
        var data = await _dashboardService.GetDoctorSummaryAsync(
            doctorId,
            from.ToUniversalTime(),
            to.ToUniversalTime(),
            ct
        );
        return Ok(ApiResponse.Success(data));
    }

    [HttpGet("doctor/appointments/series")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorAppointmentSeries(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to,
        [FromQuery] string bucket = "day",
        CancellationToken ct = default
    )
    {
        var doctorIdStr = User.FindFirst("doctorId")?.Value;
        if (!int.TryParse(doctorIdStr, out var doctorId))
            return Forbid();
        var data = await _dashboardService.GetAppointmentSeriesAsync(
            from.ToUniversalTime(),
            to.ToUniversalTime(),
            bucket,
            doctorId,
            ct
        );
        return Ok(ApiResponse.Success(data));
    }

    [HttpGet("admin/appointments/recent")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRecentAppointments(
        [FromQuery] int limit = 10,
        CancellationToken ct = default
    )
    {
        var data = await _dashboardService.GetRecentAppointmentsAsync(limit, ct);
        return Ok(ApiResponse.Success(data));
    }
}
