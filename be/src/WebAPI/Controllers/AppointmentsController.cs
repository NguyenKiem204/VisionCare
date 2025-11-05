using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.AppointmentDto;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }


    [HttpGet]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments()
    {
        var appointments = await _appointmentService.GetAllAppointmentsAsync();
        return Ok(ApiResponse<IEnumerable<AppointmentDto>>.Ok(appointments));
    }

    [HttpGet("search")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> SearchAppointments(
        [FromQuery] string? keyword,
        [FromQuery] string? status,
        [FromQuery] int? doctorId,
        [FromQuery] int? customerId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool desc = false
    )
    {
        var result = await _appointmentService.SearchAppointmentsAsync(
            keyword,
            status,
            doctorId,
            customerId,
            startDate,
            endDate,
            page,
            pageSize,
            sortBy,
            desc
        );
        return Ok(PagedResponse<AppointmentDto>.Ok(result.items, result.totalCount, page, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
    {
        var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }
        return Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment(
        CreateAppointmentRequest request
    )
    {
        var appointment = await _appointmentService.CreateAppointmentAsync(request);
        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AppointmentDto>> UpdateAppointment(
        int id,
        UpdateAppointmentRequest request
    )
    {
        var appointment = await _appointmentService.UpdateAppointmentAsync(id, request);
        return Ok(appointment);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAppointment(int id)
    {
        var result = await _appointmentService.DeleteAppointmentAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDoctor(
        int doctorId,
        [FromQuery] DateTime? date = null
    )
    {
        var appointments = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId, date);
        return Ok(appointments);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByCustomer(
        int customerId,
        [FromQuery] DateTime? date = null
    )
    {
        var appointments = await _appointmentService.GetAppointmentsByCustomerAsync(
            customerId,
            date
        );
        return Ok(appointments);
    }

    [HttpGet("date/{date}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDate(
        DateTime date
    )
    {
        var appointments = await _appointmentService.GetAppointmentsByDateAsync(date);
        return Ok(appointments);
    }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate
    )
    {
        var appointments = await _appointmentService.GetAppointmentsByDateRangeAsync(
            startDate,
            endDate
        );
        return Ok(appointments);
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetUpcomingAppointments(
        [FromQuery] int? doctorId = null,
        [FromQuery] int? customerId = null
    )
    {
        var appointments = await _appointmentService.GetUpcomingAppointmentsAsync(
            doctorId,
            customerId
        );
        return Ok(appointments);
    }

    [HttpPut("{id}/confirm")]
    public async Task<ActionResult<AppointmentDto>> ConfirmAppointment(int id)
    {
        var appointment = await _appointmentService.ConfirmAppointmentAsync(id);
        return Ok(appointment);
    }

  
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<AppointmentDto>> CancelAppointment(
        int id,
        [FromBody] string? reason = null
    )
    {
        var appointment = await _appointmentService.CancelAppointmentAsync(id, reason);
        return Ok(appointment);
    }

    [HttpPut("{id}/complete")]
    public async Task<ActionResult<AppointmentDto>> CompleteAppointment(
        int id,
        [FromBody] string? notes = null
    )
    {
        var appointment = await _appointmentService.CompleteAppointmentAsync(id, notes);
        return Ok(appointment);
    }

    [HttpPut("{id}/reschedule")]
    public async Task<ActionResult<AppointmentDto>> RescheduleAppointment(
        int id,
        [FromBody] DateTime newDateTime
    )
    {
        var appointment = await _appointmentService.RescheduleAppointmentAsync(id, newDateTime);
        return Ok(appointment);
    }

    [HttpGet("availability")]
    public async Task<ActionResult<bool>> CheckDoctorAvailability(
        [FromQuery] int doctorId,
        [FromQuery] DateTime dateTime
    )
    {
        var isAvailable = await _appointmentService.IsDoctorAvailableAsync(doctorId, dateTime);
        return Ok(isAvailable);
    }

    [HttpGet("available-slots")]
    public async Task<ActionResult<IEnumerable<DateTime>>> GetAvailableTimeSlots(
        [FromQuery] int doctorId,
        [FromQuery] DateTime date
    )
    {
        var slots = await _appointmentService.GetAvailableTimeSlotsAsync(doctorId, date);
        return Ok(slots);
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetOverdueAppointments()
    {
        var appointments = await _appointmentService.GetOverdueAppointmentsAsync();
        return Ok(appointments);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult<object>> GetAppointmentStatistics()
    {
        var totalCount = await _appointmentService.GetTotalAppointmentsCountAsync();
        var statusStats = await _appointmentService.GetAppointmentsByStatusStatsAsync();
        var doctorStats = await _appointmentService.GetAppointmentsByDoctorStatsAsync();

        return Ok(
            new
            {
                TotalCount = totalCount,
                StatusStats = statusStats,
                DoctorStats = doctorStats,
            }
        );
    }
}
