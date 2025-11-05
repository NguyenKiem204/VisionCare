using AutoMapper;
using VisionCare.Application.DTOs;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Scheduling;

public class DoctorAbsenceService : IDoctorAbsenceService
{
    private readonly IDoctorAbsenceRepository _absenceRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ISubstituteDoctorService _substituteDoctorService;
    private readonly IMapper _mapper;

    public DoctorAbsenceService(
        IDoctorAbsenceRepository absenceRepository,
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository,
        IScheduleRepository scheduleRepository,
        ISubstituteDoctorService substituteDoctorService,
        IMapper mapper
    )
    {
        _absenceRepository = absenceRepository;
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
        _scheduleRepository = scheduleRepository;
        _substituteDoctorService = substituteDoctorService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorAbsenceDto>> GetAllAsync()
    {
        var absences = await _absenceRepository.GetAllAsync();
        return await MapToDtosAsync(absences);
    }

    public async Task<DoctorAbsenceDto?> GetByIdAsync(int id)
    {
        var absence = await _absenceRepository.GetByIdAsync(id);
        if (absence == null)
            return null;

        return await MapToDtoAsync(absence);
    }

    public async Task<IEnumerable<DoctorAbsenceDto>> GetByDoctorIdAsync(int doctorId)
    {
        var absences = await _absenceRepository.GetByDoctorIdAsync(doctorId);
        return await MapToDtosAsync(absences);
    }

    public async Task<IEnumerable<DoctorAbsenceDto>> GetPendingAsync()
    {
        var absences = await _absenceRepository.GetPendingAsync();
        return await MapToDtosAsync(absences);
    }

    public async Task<DoctorAbsenceDto> CreateAsync(CreateDoctorAbsenceRequest request)
    {
        // Validate doctor exists
        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
        if (doctor == null)
        {
            throw new NotFoundException($"Doctor with ID {request.DoctorId} not found.");
        }

        // Validate date range
        if (request.EndDate < request.StartDate)
        {
            throw new ValidationException("End date must be greater than or equal to start date.");
        }

        var absence = new DoctorAbsence
        {
            DoctorId = request.DoctorId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AbsenceType = request.AbsenceType,
            Reason = request.Reason,
            Status = "Pending",
            IsResolved = false,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };

        var created = await _absenceRepository.AddAsync(absence);
        return await MapToDtoAsync(created);
    }

    public async Task<DoctorAbsenceDto> UpdateAsync(int id, UpdateDoctorAbsenceRequest request)
    {
        var existing = await _absenceRepository.GetByIdAsync(id);
        if (existing == null)
        {
            throw new NotFoundException($"Doctor absence with ID {id} not found.");
        }

        if (request.StartDate.HasValue)
            existing.StartDate = request.StartDate.Value;

        if (request.EndDate.HasValue)
            existing.EndDate = request.EndDate.Value;

        if (!string.IsNullOrEmpty(request.AbsenceType))
            existing.AbsenceType = request.AbsenceType;

        if (request.Reason != null)
            existing.Reason = request.Reason;

        if (!string.IsNullOrEmpty(request.Status))
        {
            if (request.Status == "Approved")
                existing.Approve();
            else if (request.Status == "Rejected")
                existing.Reject();
            else
                existing.Status = request.Status;
        }

        await _absenceRepository.UpdateAsync(existing);
        return await MapToDtoAsync(existing);
    }

    public async Task<DoctorAbsenceDto> ApproveAsync(int id)
    {
        var absence = await _absenceRepository.GetByIdAsync(id);
        if (absence == null)
        {
            throw new NotFoundException($"Doctor absence with ID {id} not found.");
        }

        absence.Approve();
        await _absenceRepository.UpdateAsync(absence);

        // Automatically handle appointments if not resolved
        if (!absence.IsResolved)
        {
            await HandleAbsenceAppointmentsAsync(
                id,
                new HandleAbsenceAppointmentsRequest { AutoAssignSubstitute = true }
            );
        }

        return await MapToDtoAsync(absence);
    }

    public async Task<DoctorAbsenceDto> RejectAsync(int id)
    {
        var absence = await _absenceRepository.GetByIdAsync(id);
        if (absence == null)
        {
            throw new NotFoundException($"Doctor absence with ID {id} not found.");
        }

        absence.Reject();
        await _absenceRepository.UpdateAsync(absence);
        return await MapToDtoAsync(absence);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _absenceRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        await _absenceRepository.DeleteAsync(id);
        return true;
    }

    public async Task<Dictionary<string, int>> HandleAbsenceAppointmentsAsync(
        int absenceId,
        HandleAbsenceAppointmentsRequest request
    )
    {
        var absence = await _absenceRepository.GetByIdAsync(absenceId);
        if (absence == null)
        {
            throw new NotFoundException($"Doctor absence with ID {absenceId} not found.");
        }

        if (absence.Status != "Approved")
        {
            throw new ValidationException("Can only handle appointments for approved absences.");
        }

        // Get all appointments in the absence date range
        var startDateTime = absence.StartDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = absence.EndDate.ToDateTime(TimeOnly.MaxValue);

        var appointments = await _appointmentRepository.GetByDateRangeAsync(
            startDateTime,
            endDateTime
        );
        var affectedAppointments = appointments
            .Where(
                a =>
                    a.DoctorId == absence.DoctorId
                    && a.AppointmentStatus != "Cancelled"
                    && a.AppointmentStatus != "Completed"
            )
            .ToList();

        int transferred = 0;
        int cancelled = 0;

        // Block all schedules in the absence period
        var currentDate = absence.StartDate;
        while (currentDate <= absence.EndDate)
        {
            var schedules = await _scheduleRepository.GetByDoctorAndDateAsync(
                absence.DoctorId,
                currentDate
            );

            foreach (var schedule in schedules)
            {
                if (schedule.Status == "Available")
                {
                    schedule.Block("Doctor absence");
                    await _scheduleRepository.UpdateAsync(schedule);
                }
            }

            currentDate = currentDate.AddDays(1);
        }

        // Handle each appointment
        foreach (var appointment in affectedAppointments)
        {
            // Manual assignment takes precedence
            if (
                request.ManualSubstituteAssignments != null
                && request.ManualSubstituteAssignments.ContainsKey(appointment.Id)
            )
            {
                var substituteDoctorId = request.ManualSubstituteAssignments[appointment.Id];
                await TransferAppointmentAsync(appointment, substituteDoctorId);
                transferred++;
            }
            // Cancel if explicitly requested
            else if (
                request.AppointmentIdsToCancel != null
                && request.AppointmentIdsToCancel.Contains(appointment.Id)
            )
            {
                appointment.Cancel($"Doctor absence: {absence.Reason}");
                await _appointmentRepository.UpdateAsync(appointment);
                cancelled++;
            }
            // Auto assign substitute if enabled
            else if (request.AutoAssignSubstitute)
            {
                var substituteDoctor = await _substituteDoctorService.FindSubstituteAsync(
                    absence.DoctorId,
                    appointment.AppointmentDate ?? DateTime.MinValue
                );

                if (substituteDoctor != null)
                {
                    await TransferAppointmentAsync(appointment, substituteDoctor.Id);
                    transferred++;
                }
                else
                {
                    appointment.Cancel($"Doctor absence: {absence.Reason}. No substitute doctor available.");
                    await _appointmentRepository.UpdateAsync(appointment);
                    cancelled++;
                }
            }
            else
            {
                // No action specified, cancel by default
                appointment.Cancel($"Doctor absence: {absence.Reason}");
                await _appointmentRepository.UpdateAsync(appointment);
                cancelled++;
            }
        }

        // Mark absence as resolved
        absence.MarkAsResolved();
        await _absenceRepository.UpdateAsync(absence);

        return new Dictionary<string, int>
        {
            { "transferred", transferred },
            { "cancelled", cancelled },
            { "total", affectedAppointments.Count }
        };
    }

    private async Task TransferAppointmentAsync(Appointment appointment, int substituteDoctorId)
    {
        appointment.DoctorId = substituteDoctorId;
        appointment.Notes =
            (appointment.Notes ?? "")
            + $"\n[Transferred] Original doctor unavailable. Transferred to doctor ID {substituteDoctorId}.";
        await _appointmentRepository.UpdateAsync(appointment);
    }

    private async Task<DoctorAbsenceDto> MapToDtoAsync(DoctorAbsence absence)
    {
        var doctor = await _doctorRepository.GetByIdAsync(absence.DoctorId);
        return new DoctorAbsenceDto
        {
            Id = absence.Id,
            DoctorId = absence.DoctorId,
            DoctorName = doctor?.DoctorName ?? "Unknown",
            StartDate = absence.StartDate,
            EndDate = absence.EndDate,
            AbsenceType = absence.AbsenceType,
            Reason = absence.Reason,
            Status = absence.Status,
            IsResolved = absence.IsResolved,
            Created = absence.Created,
            LastModified = absence.LastModified,
        };
    }

    private async Task<IEnumerable<DoctorAbsenceDto>> MapToDtosAsync(
        IEnumerable<DoctorAbsence> absences
    )
    {
        var dtos = new List<DoctorAbsenceDto>();
        foreach (var absence in absences)
        {
            dtos.Add(await MapToDtoAsync(absence));
        }
        return dtos;
    }
}

