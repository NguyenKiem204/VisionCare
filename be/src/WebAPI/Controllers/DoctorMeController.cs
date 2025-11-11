using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs;
using VisionCare.Application.DTOs.AppointmentDto;
using VisionCare.Application.DTOs.CustomerDto;
using VisionCare.Application.DTOs.DoctorScheduleDto;
using VisionCare.Application.DTOs.ScheduleDto;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/doctor/me")]
[Authorize(Policy = "DoctorOnly")]
public class DoctorMeController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    private readonly IWeeklyScheduleService _weeklyScheduleService;
    private readonly IDoctorScheduleService _doctorScheduleService;
    private readonly IDoctorAbsenceService _absenceService;
    private readonly IAppointmentService _appointmentService;

    public DoctorMeController(
        IScheduleService scheduleService,
        IWeeklyScheduleService weeklyScheduleService,
        IDoctorScheduleService doctorScheduleService,
        IDoctorAbsenceService absenceService,
        IAppointmentService appointmentService
    )
    {
        _scheduleService = scheduleService;
        _weeklyScheduleService = weeklyScheduleService;
        _doctorScheduleService = doctorScheduleService;
        _absenceService = absenceService;
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

    // ===== Appointment status self-service (guarded to own appointments) =====
    [HttpPut("appointments/{id}/confirm")]
    public async Task<ActionResult<AppointmentDto>> ConfirmMyAppointment(int id)
    {
        var doctorId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.DoctorId != doctorId)
        {
            return Forbid();
        }
        var updated = await _appointmentService.ConfirmAppointmentAsync(id);
        return Ok(ApiResponse<AppointmentDto>.Ok(updated));
    }

    // Patients of current doctor (unique by customer)
    [HttpGet("patients")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetMyPatients(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null
    )
    {
        var doctorId = GetCurrentAccountId();
        IEnumerable<AppointmentDto> items;
        if (from.HasValue && to.HasValue)
        {
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
            items = result.items;
        }
        else
        {
            items = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId, null);
        }

        var patients = items
            .Where(a => a.PatientId.HasValue)
            .GroupBy(a => a.PatientId!.Value)
            .Select(g => new CustomerDto
            {
                Id = g.Key,
                CustomerName = g.First().PatientName ?? $"Bệnh nhân #{g.Key}",
                Phone = null,
                Address = null,
                Dob = null,
                Gender = null,
            })
            .ToList();

        return Ok(ApiResponse<IEnumerable<CustomerDto>>.Ok(patients));
    }

    [HttpPut("appointments/{id}/complete")]
    public async Task<ActionResult<AppointmentDto>> CompleteMyAppointment(
        int id,
        [FromBody] string? notes = null
    )
    {
        var doctorId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.DoctorId != doctorId)
        {
            return Forbid();
        }
        var updated = await _appointmentService.CompleteAppointmentAsync(id, notes);
        return Ok(ApiResponse<AppointmentDto>.Ok(updated));
    }

    [HttpPut("appointments/{id}/cancel")]
    public async Task<ActionResult<AppointmentDto>> CancelMyAppointment(
        int id,
        [FromBody] string? reason = null
    )
    {
        var doctorId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.DoctorId != doctorId)
        {
            return Forbid();
        }
        var updated = await _appointmentService.CancelAppointmentAsync(id, reason);
        return Ok(ApiResponse<AppointmentDto>.Ok(updated));
    }

    [HttpGet("weekly-schedules")]
    public async Task<ActionResult<IEnumerable<WeeklyScheduleDto>>> GetMyWeeklySchedules()
    {
        var doctorId = GetCurrentAccountId();
        var schedules = await _weeklyScheduleService.GetActiveByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<WeeklyScheduleDto>>.Ok(schedules));
    }

    [HttpGet("doctor-schedules")]
    public async Task<ActionResult<IEnumerable<DoctorScheduleDto>>> GetMyDoctorSchedules()
    {
        var doctorId = GetCurrentAccountId();
        var schedules = await _doctorScheduleService.GetActiveDoctorSchedulesByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<DoctorScheduleDto>>.Ok(schedules));
    }

    [HttpGet("schedules")]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetMySchedules(
        [FromQuery] DateTime? date,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to
    )
    {
        var doctorId = GetCurrentAccountId();

        if (date.HasValue)
        {
            var list = await _scheduleService.GetSchedulesByDoctorAndDateAsync(
                doctorId,
                DateOnly.FromDateTime(date.Value)
            );
            return Ok(ApiResponse<IEnumerable<ScheduleDto>>.Ok(list));
        }

        if (from.HasValue && to.HasValue)
        {
            var results = new List<ScheduleDto>();
            for (var d = from.Value; d <= to.Value; d = d.AddDays(1))
            {
                var daily = await _scheduleService.GetSchedulesByDoctorAndDateAsync(doctorId, d);
                results.AddRange(daily);
            }
            return Ok(ApiResponse<IEnumerable<ScheduleDto>>.Ok(results));
        }

        var all = await _scheduleService.GetSchedulesByDoctorAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<ScheduleDto>>.Ok(all));
    }

    [HttpGet("upcoming-appointments")]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetMyUpcoming(
        [FromQuery] int limit = 10
    )
    {
        var doctorId = GetCurrentAccountId();
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var list = await _scheduleService.GetSchedulesByDoctorAndDateAsync(doctorId, today);
        var upcoming = list.OrderBy(s => s.ScheduleDate)
            .ThenBy(s => s.StartTime)
            .Take(Math.Max(1, Math.Min(limit, 50)))
            .ToList();
        return Ok(ApiResponse<IEnumerable<ScheduleDto>>.Ok(upcoming));
    }

    [HttpGet("absences")]
    public async Task<ActionResult<IEnumerable<DoctorAbsenceDto>>> GetMyAbsences()
    {
        var doctorId = GetCurrentAccountId();
        var absences = await _absenceService.GetByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<DoctorAbsenceDto>>.Ok(absences));
    }

    [HttpPost("absences")]
    public async Task<ActionResult<DoctorAbsenceDto>> CreateMyAbsence(
        [FromBody] CreateDoctorAbsenceRequest request
    )
    {
        var doctorId = GetCurrentAccountId();
        var payload = new CreateDoctorAbsenceRequest
        {
            DoctorId = doctorId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AbsenceType = string.IsNullOrWhiteSpace(request.AbsenceType)
                ? "Leave"
                : request.AbsenceType,
            Reason = request.Reason ?? string.Empty,
        };

        var created = await _absenceService.CreateAsync(payload);
        return CreatedAtAction(nameof(GetMyAbsences), ApiResponse<DoctorAbsenceDto>.Ok(created));
    }
}
