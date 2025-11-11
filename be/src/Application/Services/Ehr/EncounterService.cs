using VisionCare.Application.DTOs.Ehr;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Ehr;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Ehr;

public class EncounterService : IEncounterService
{
    private readonly IEncounterRepository _encounterRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public EncounterService(
        IEncounterRepository encounterRepository,
        IAppointmentRepository appointmentRepository
    )
    {
        _encounterRepository = encounterRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<EncounterDto?> GetByIdAsync(int id)
    {
        var e = await _encounterRepository.GetByIdAsync(id);
        return e == null ? null : Map(e);
    }

    public async Task<IEnumerable<EncounterDto>> GetByDoctorAndRangeAsync(
        int doctorId,
        DateOnly? from,
        DateOnly? to
    )
    {
        var list = await _encounterRepository.GetByDoctorAndRangeAsync(doctorId, from, to);
        return list.Select(Map).ToList();
    }

    public async Task<EncounterDto> CreateAsync(
        int doctorId,
        int customerId,
        CreateEncounterRequest request
    )
    {
        var appt = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
        if (appt == null)
            throw new NotFoundException("Appointment not found");
        if (appt.DoctorId != doctorId)
            throw new UnauthorizedAccessException("Cannot create encounter for another doctor");
        if (appt.PatientId != customerId)
            throw new ValidationException("Customer does not match appointment");

        var entity = new Encounter
        {
            AppointmentId = request.AppointmentId,
            DoctorId = doctorId,
            CustomerId = customerId,
            Subjective = request.Subjective,
            Objective = request.Objective,
            Assessment = request.Assessment,
            Plan = request.Plan,
            Status = "Draft",
            CreatedAt = DateTime.UtcNow,
        };
        var created = await _encounterRepository.AddAsync(entity);
        return Map(created);
    }

    public async Task<EncounterDto> UpdateAsync(
        int id,
        int doctorId,
        UpdateEncounterRequest request
    )
    {
        var e = await _encounterRepository.GetByIdAsync(id);
        if (e == null)
            throw new NotFoundException("Encounter not found");
        if (e.DoctorId != doctorId)
            throw new UnauthorizedAccessException("Cannot modify encounter of another doctor");
        if (string.Equals(e.Status, "Signed", StringComparison.OrdinalIgnoreCase))
            throw new ValidationException("Signed encounters cannot be modified");

        if (request.Subjective != null) e.Subjective = request.Subjective;
        if (request.Objective != null) e.Objective = request.Objective;
        if (request.Assessment != null) e.Assessment = request.Assessment;
        if (request.Plan != null) e.Plan = request.Plan;
        if (!string.IsNullOrWhiteSpace(request.Status)) e.Status = request.Status!;
        e.UpdatedAt = DateTime.UtcNow;
        await _encounterRepository.UpdateAsync(e);
        return Map(e);
    }

    private static EncounterDto Map(Encounter e)
    {
        return new EncounterDto
        {
            Id = e.Id,
            AppointmentId = e.AppointmentId,
            DoctorId = e.DoctorId,
            CustomerId = e.CustomerId,
            Subjective = e.Subjective,
            Objective = e.Objective,
            Assessment = e.Assessment,
            Plan = e.Plan,
            Status = e.Status,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
        };
    }
}


