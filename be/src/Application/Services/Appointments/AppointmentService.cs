using AutoMapper;
using VisionCare.Application.DTOs.AppointmentDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Appointments;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        ICustomerRepository customerRepository,
        IMapper mapper
    )
    {
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _appointmentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        return appointment != null ? _mapper.Map<AppointmentDto>(appointment) : null;
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentRequest request)
    {
        // Validate doctor exists
        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
        if (doctor == null)
        {
            throw new NotFoundException($"Doctor with ID {request.DoctorId} not found.");
        }

        // Validate customer exists
        var customer = await _customerRepository.GetByIdAsync(request.PatientId);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with ID {request.PatientId} not found.");
        }

        // Check if doctor is available at the requested time
        if (!await IsDoctorAvailableAsync(request.DoctorId, request.AppointmentDate))
        {
            throw new ValidationException("Doctor is not available at the requested time.");
        }

        // Use AutoMapper to create entity from DTO
        var appointment = _mapper.Map<Appointment>(request);
        appointment.AppointmentStatus = "Pending";
        appointment.Created = DateTime.UtcNow;

        var createdAppointment = await _appointmentRepository.AddAsync(appointment);
        return _mapper.Map<AppointmentDto>(createdAppointment);
    }

    public async Task<AppointmentDto> UpdateAppointmentAsync(
        int id,
        UpdateAppointmentRequest request
    )
    {
        var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
        if (existingAppointment == null)
        {
            throw new NotFoundException($"Appointment with ID {id} not found.");
        }

        // If changing date/time, check availability
        if (
            request.AppointmentDate.HasValue
            && request.AppointmentDate != existingAppointment.AppointmentDate
        )
        {
            if (
                !await IsDoctorAvailableAsync(
                    existingAppointment.DoctorId ?? 0,
                    request.AppointmentDate.Value
                )
            )
            {
                throw new ValidationException("Doctor is not available at the requested time.");
            }
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingAppointment);

        await _appointmentRepository.UpdateAsync(existingAppointment);
        return _mapper.Map<AppointmentDto>(existingAppointment);
    }

    public async Task<bool> DeleteAppointmentAsync(int id)
    {
        var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
        if (existingAppointment == null)
        {
            return false;
        }

        await _appointmentRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(
        int doctorId,
        DateTime? date = null
    )
    {
        var appointments = await _appointmentRepository.GetByDoctorAsync(doctorId, date);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByCustomerAsync(
        int customerId,
        DateTime? date = null
    )
    {
        var appointments = await _appointmentRepository.GetByCustomerAsync(customerId, date);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateAsync(DateTime date)
    {
        var appointments = await _appointmentRepository.GetByDateAsync(date);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateRangeAsync(
        DateTime startDate,
        DateTime endDate
    )
    {
        var appointments = await _appointmentRepository.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(
        int? doctorId = null,
        int? customerId = null
    )
    {
        var appointments = await _appointmentRepository.GetUpcomingAsync(doctorId, customerId);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        // Use domain method to confirm appointment
        appointment.Confirm();

        await _appointmentRepository.UpdateAsync(appointment);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> CancelAppointmentAsync(
        int appointmentId,
        string? reason = null
    )
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        // Use domain method to cancel appointment
        appointment.Cancel(reason);

        await _appointmentRepository.UpdateAsync(appointment);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> CompleteAppointmentAsync(
        int appointmentId,
        string? notes = null
    )
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        // Use domain method to complete appointment
        appointment.Complete(notes);

        await _appointmentRepository.UpdateAsync(appointment);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> RescheduleAppointmentAsync(
        int appointmentId,
        DateTime newDateTime
    )
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        if (!await IsDoctorAvailableAsync(appointment.DoctorId ?? 0, newDateTime))
        {
            throw new ValidationException("Doctor is not available at the requested time.");
        }

        // Use domain method to reschedule appointment
        appointment.Reschedule(newDateTime);

        await _appointmentRepository.UpdateAsync(appointment);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime dateTime)
    {
        return await _appointmentRepository.IsDoctorAvailableAsync(doctorId, dateTime);
    }

    public async Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date)
    {
        return await _appointmentRepository.GetAvailableTimeSlotsAsync(doctorId, date);
    }

    public async Task<bool> CheckAppointmentConflictAsync(
        int doctorId,
        DateTime dateTime,
        int? excludeAppointmentId = null
    )
    {
        return await _appointmentRepository.CheckConflictAsync(
            doctorId,
            dateTime,
            excludeAppointmentId
        );
    }

    public async Task<int> GetTotalAppointmentsCountAsync()
    {
        return await _appointmentRepository.GetTotalCountAsync();
    }

    public async Task<Dictionary<string, int>> GetAppointmentsByStatusStatsAsync()
    {
        return await _appointmentRepository.GetStatusStatsAsync();
    }

    public async Task<Dictionary<string, int>> GetAppointmentsByDoctorStatsAsync()
    {
        return await _appointmentRepository.GetDoctorStatsAsync();
    }

    public async Task<IEnumerable<AppointmentDto>> GetOverdueAppointmentsAsync()
    {
        var appointments = await _appointmentRepository.GetOverdueAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }
}
