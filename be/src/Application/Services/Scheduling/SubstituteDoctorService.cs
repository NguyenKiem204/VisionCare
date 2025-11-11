using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Scheduling;

public class SubstituteDoctorService : ISubstituteDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public SubstituteDoctorService(
        IDoctorRepository doctorRepository,
        IScheduleRepository scheduleRepository,
        IAppointmentRepository appointmentRepository
    )
    {
        _doctorRepository = doctorRepository;
        _scheduleRepository = scheduleRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Doctor?> FindSubstituteAsync(
        int originalDoctorId,
        DateTime appointmentDateTime
    )
    {
        // Get original doctor to find doctors with same specialization
        var originalDoctor = await _doctorRepository.GetByIdAsync(originalDoctorId);
        if (originalDoctor == null || !originalDoctor.SpecializationId.HasValue)
            return null;

        // Get all doctors with same specialization
        var doctors = await _doctorRepository.GetBySpecializationAsync(
            originalDoctor.SpecializationId.Value
        );

        // Exclude the original doctor
        doctors = doctors.Where(d => (d.AccountId ?? 0) != originalDoctorId);

        var appointmentDate = DateOnly.FromDateTime(appointmentDateTime);
        var appointmentTime = TimeOnly.FromDateTime(appointmentDateTime);

        // Find doctors available at the appointment time
        var availableDoctors = new List<(Doctor doctor, int appointmentCount)>();

        foreach (var doctor in doctors)
        {
            var doctorId = doctor.AccountId ?? 0;
            if (doctorId == 0) continue;

            // Check if doctor has schedule for this date and time
            var schedules = await _scheduleRepository.GetByDoctorAndDateAsync(
                doctorId,
                appointmentDate
            );

            var availableSchedule = schedules.FirstOrDefault(s =>
            {
                // Need to check if slot time matches
                // This is simplified - in real implementation, we'd need to check slot times
                return s.Status == "Available" && s.IsValidForBooking();
            });

            if (availableSchedule != null)
            {
                // Count appointments on this date to prefer less busy doctors
                var appointments = await _appointmentRepository.GetByDoctorAsync(
                    doctorId,
                    appointmentDateTime
                );
                var appointmentCount = appointments.Count(a =>
                    a.AppointmentStatus != "Cancelled" && a.AppointmentStatus != "Completed"
                );

                availableDoctors.Add((doctor, appointmentCount));
            }
        }

        // Return doctor with least appointments (most available)
        return availableDoctors
            .OrderBy(d => d.appointmentCount)
            .ThenBy(d => d.doctor.Rating ?? 0)
            .FirstOrDefault()
            .doctor;
    }
}

