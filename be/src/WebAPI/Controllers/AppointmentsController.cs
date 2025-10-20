using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.AppointmentDto;
using VisionCare.Application.Interfaces.Appointments;

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

    /// <summary>
    /// Get all appointments
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments()
    {
        var appointments = await _appointmentService.GetAllAppointmentsAsync();
        return Ok(appointments);
    }

    /// <summary>
    /// Get appointment by ID
    /// </summary>
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

    /// <summary>
    /// Create a new appointment
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment(
        CreateAppointmentRequest request
    )
    {
        var appointment = await _appointmentService.CreateAppointmentAsync(request);
        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
    }

    /// <summary>
    /// Update appointment
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<AppointmentDto>> UpdateAppointment(
        int id,
        UpdateAppointmentRequest request
    )
    {
        var appointment = await _appointmentService.UpdateAppointmentAsync(id, request);
        return Ok(appointment);
    }

    /// <summary>
    /// Delete appointment
    /// </summary>
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

    /// <summary>
    /// Get appointments by doctor
    /// </summary>
    [HttpGet("doctor/{doctorId}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDoctor(
        int doctorId,
        [FromQuery] DateTime? date = null
    )
    {
        var appointments = await _appointmentService.GetAppointmentsByDoctorAsync(doctorId, date);
        return Ok(appointments);
    }

    /// <summary>
    /// Get appointments by customer
    /// </summary>
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

    /// <summary>
    /// Get appointments by date
    /// </summary>
    [HttpGet("date/{date}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDate(
        DateTime date
    )
    {
        var appointments = await _appointmentService.GetAppointmentsByDateAsync(date);
        return Ok(appointments);
    }

    /// <summary>
    /// Get appointments by date range
    /// </summary>
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

    /// <summary>
    /// Get upcoming appointments
    /// </summary>
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

    /// <summary>
    /// Confirm appointment
    /// </summary>
    [HttpPut("{id}/confirm")]
    public async Task<ActionResult<AppointmentDto>> ConfirmAppointment(int id)
    {
        var appointment = await _appointmentService.ConfirmAppointmentAsync(id);
        return Ok(appointment);
    }

    /// <summary>
    /// Cancel appointment
    /// </summary>
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<AppointmentDto>> CancelAppointment(
        int id,
        [FromBody] string? reason = null
    )
    {
        var appointment = await _appointmentService.CancelAppointmentAsync(id, reason);
        return Ok(appointment);
    }

    /// <summary>
    /// Complete appointment
    /// </summary>
    [HttpPut("{id}/complete")]
    public async Task<ActionResult<AppointmentDto>> CompleteAppointment(
        int id,
        [FromBody] string? notes = null
    )
    {
        var appointment = await _appointmentService.CompleteAppointmentAsync(id, notes);
        return Ok(appointment);
    }

    /// <summary>
    /// Reschedule appointment
    /// </summary>
    [HttpPut("{id}/reschedule")]
    public async Task<ActionResult<AppointmentDto>> RescheduleAppointment(
        int id,
        [FromBody] DateTime newDateTime
    )
    {
        var appointment = await _appointmentService.RescheduleAppointmentAsync(id, newDateTime);
        return Ok(appointment);
    }

    /// <summary>
    /// Check if doctor is available
    /// </summary>
    [HttpGet("availability")]
    public async Task<ActionResult<bool>> CheckDoctorAvailability(
        [FromQuery] int doctorId,
        [FromQuery] DateTime dateTime
    )
    {
        var isAvailable = await _appointmentService.IsDoctorAvailableAsync(doctorId, dateTime);
        return Ok(isAvailable);
    }

    /// <summary>
    /// Get available time slots for a doctor on a specific date
    /// </summary>
    [HttpGet("available-slots")]
    public async Task<ActionResult<IEnumerable<DateTime>>> GetAvailableTimeSlots(
        [FromQuery] int doctorId,
        [FromQuery] DateTime date
    )
    {
        var slots = await _appointmentService.GetAvailableTimeSlotsAsync(doctorId, date);
        return Ok(slots);
    }

    /// <summary>
    /// Get overdue appointments
    /// </summary>
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetOverdueAppointments()
    {
        var appointments = await _appointmentService.GetOverdueAppointmentsAsync();
        return Ok(appointments);
    }

    /// <summary>
    /// Get appointment statistics
    /// </summary>
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
