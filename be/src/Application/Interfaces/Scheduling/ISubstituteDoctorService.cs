using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface ISubstituteDoctorService
{
    Task<Doctor?> FindSubstituteAsync(int originalDoctorId, DateTime appointmentDateTime);
}

