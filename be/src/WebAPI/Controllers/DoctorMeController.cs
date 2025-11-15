using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VisionCare.Application.DTOs;
using VisionCare.Application.DTOs.AppointmentDto;
using VisionCare.Application.DTOs.CustomerDto;
using VisionCare.Application.DTOs.DoctorScheduleDto;
using VisionCare.Application.DTOs.MedicalHistoryDto;
using VisionCare.Application.DTOs.ScheduleDto;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.Application.Interfaces.Customers;
using VisionCare.Application.Interfaces.MedicalHistory;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.WebAPI.Hubs;
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
    private readonly ICustomerService _customerService;
    private readonly IMedicalHistoryService _medicalHistoryService;
    private readonly IHubContext<BookingHub> _hubContext;

    public DoctorMeController(
        IScheduleService scheduleService,
        IWeeklyScheduleService weeklyScheduleService,
        IDoctorScheduleService doctorScheduleService,
        IDoctorAbsenceService absenceService,
        IAppointmentService appointmentService,
        ICustomerService customerService,
        IMedicalHistoryService medicalHistoryService,
        IHubContext<BookingHub> hubContext
    )
    {
        _scheduleService = scheduleService;
        _weeklyScheduleService = weeklyScheduleService;
        _doctorScheduleService = doctorScheduleService;
        _absenceService = absenceService;
        _appointmentService = appointmentService;
        _customerService = customerService;
        _medicalHistoryService = medicalHistoryService;
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

    // Patients of current doctor (unique by customer) with pagination and search
    [HttpGet("patients")]
    public async Task<ActionResult<PagedResponse<CustomerDto>>> GetMyPatients(
        [FromQuery] string? keyword = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
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

        var patientGroups = items
            .Where(a => a.PatientId.HasValue)
            .GroupBy(a => a.PatientId!.Value)
            .ToList();

        var allPatients = new List<CustomerDto>();
        
        foreach (var group in patientGroups)
        {
            var patientId = group.Key;
            var appointments = group.ToList();
            var firstAppointment = appointments.First();
            
            // Load full customer details
            var customerDto = await _customerService.GetCustomerByIdAsync(patientId);
            
            if (customerDto == null)
            {
                // Fallback if customer not found
                customerDto = new CustomerDto
                {
                    Id = patientId,
                    CustomerName = firstAppointment.PatientName ?? $"Bệnh nhân #{patientId}",
                };
            }
            else
            {
                // Ensure name is set
                if (string.IsNullOrEmpty(customerDto.CustomerName))
                {
                    customerDto.CustomerName = firstAppointment.PatientName ?? $"Bệnh nhân #{patientId}";
                }
            }
            
            allPatients.Add(customerDto);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var keywordLower = keyword.ToLower();
            allPatients = allPatients
                .Where(p => 
                    (p.CustomerName != null && p.CustomerName.ToLower().Contains(keywordLower)) ||
                    (p.Email != null && p.Email.ToLower().Contains(keywordLower)) ||
                    (p.Phone != null && p.Phone.Contains(keyword))
                )
                .ToList();
        }

        // Sort by name
        allPatients = allPatients
            .OrderBy(p => p.CustomerName ?? $"Bệnh nhân #{p.Id}")
            .ToList();

        // Apply pagination
        var totalCount = allPatients.Count;
        var paginatedPatients = allPatients
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(PagedResponse<CustomerDto>.Ok(
            paginatedPatients,
            totalCount,
            page,
            pageSize
        ));
    }

    // Get medical history for a patient (only patients of current doctor)
    [HttpGet("patients/{patientId}/medical-history")]
    public async Task<ActionResult<IEnumerable<MedicalHistoryDto>>> GetPatientMedicalHistory(int patientId)
    {
        var doctorId = GetCurrentAccountId();
        
        // Verify this patient has appointments with current doctor
        var appointments = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId, null);
        var hasAppointment = appointments.Any(a => a.PatientId == patientId);
        
        if (!hasAppointment)
        {
            return Forbid();
        }

        var medicalHistories = await _medicalHistoryService.GetMedicalHistoriesByPatientIdAsync(patientId);
        
        // Filter to only include histories from appointments with this doctor
        var filteredHistories = medicalHistories
            .Where(mh => appointments.Any(a => a.Id == mh.AppointmentId && a.DoctorId == doctorId))
            .ToList();

        return Ok(ApiResponse<IEnumerable<MedicalHistoryDto>>.Ok(filteredHistories));
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

    [HttpGet("appointments")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetMyAppointments(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null
    )
    {
        var doctorId = GetCurrentAccountId();
        IEnumerable<AppointmentDto> appointments;
        
        if (from.HasValue && to.HasValue)
        {
            appointments = await _appointmentService.GetAppointmentsByDateRangeAsync(from.Value, to.Value);
            // Filter by doctorId since GetAppointmentsByDateRangeAsync doesn't filter by doctor
            appointments = appointments.Where(a => a.DoctorId == doctorId);
        }
        else if (from.HasValue)
        {
            var endDate = from.Value.AddDays(7); // Default to 7 days if only from is provided
            appointments = await _appointmentService.GetAppointmentsByDateRangeAsync(from.Value, endDate);
            appointments = appointments.Where(a => a.DoctorId == doctorId);
        }
        else
        {
            appointments = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId, null);
        }
        
        return Ok(ApiResponse<IEnumerable<AppointmentDto>>.Ok(appointments));
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

    // Reschedule Workflow Endpoints
    [HttpPost("appointments/{id}/request-reschedule")]
    public async Task<ActionResult<AppointmentDto>> RequestRescheduleMyAppointment(
        int id,
        [FromBody] RequestRescheduleRequest request
    )
    {
        var doctorId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.DoctorId != doctorId)
        {
            return Forbid();
        }

        try
        {
            var updated = await _appointmentService.RequestRescheduleAsync(
                id,
                request.ProposedDateTime,
                "Doctor",
                request.Reason
            );

            // Send SignalR notification
            await _hubContext.Clients
                .Group($"appointment:{id}")
                .SendAsync("RescheduleRequested", new
                {
                    appointmentId = id,
                    proposedDateTime = request.ProposedDateTime,
                    requestedBy = "Doctor",
                    reason = request.Reason
                });

            return Ok(ApiResponse<AppointmentDto>.Ok(updated));
        }
        catch (VisionCare.Application.Exceptions.ValidationException ex)
        {
            return BadRequest(ApiResponse<AppointmentDto>.Fail(ex.Message));
        }
    }

    [HttpPut("appointments/{id}/approve-reschedule")]
    public async Task<ActionResult<AppointmentDto>> ApproveRescheduleMyAppointment(int id)
    {
        var doctorId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.DoctorId != doctorId)
        {
            return Forbid();
        }

        try
        {
            var updated = await _appointmentService.ApproveRescheduleAsync(id, "Doctor");

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

    [HttpPut("appointments/{id}/reject-reschedule")]
    public async Task<ActionResult<AppointmentDto>> RejectRescheduleMyAppointment(
        int id,
        [FromBody] RejectRescheduleRequest? request = null
    )
    {
        var doctorId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.DoctorId != doctorId)
        {
            return Forbid();
        }

        try
        {
            var updated = await _appointmentService.RejectRescheduleAsync(id, "Doctor", request?.Reason);

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

    [HttpPost("appointments/{id}/counter-reschedule")]
    public async Task<ActionResult<AppointmentDto>> CounterRescheduleMyAppointment(
        int id,
        [FromBody] CounterRescheduleRequest request
    )
    {
        var doctorId = GetCurrentAccountId();
        var appt = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appt == null || appt.DoctorId != doctorId)
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
                    requestedBy = "Doctor",
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
