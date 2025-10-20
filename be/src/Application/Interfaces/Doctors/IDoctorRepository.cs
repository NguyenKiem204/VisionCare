using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces;

public interface IDoctorRepository
{
    Task<IEnumerable<Doctor>> GetAllAsync();
    Task<Doctor?> GetByIdAsync(int id);
    Task<Doctor> AddAsync(Doctor doctor);
    Task UpdateAsync(Doctor doctor);
    Task DeleteAsync(int id);

    // Additional operations for DoctorManager
    Task<IEnumerable<Doctor>> GetBySpecializationAsync(int specializationId);
    Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(DateTime date);
    Task<IEnumerable<Doctor>> SearchDoctorsAsync(
        string keyword,
        int? specializationId,
        double? minRating
    );
    Task<int> GetTotalCountAsync();
    Task<double> GetAverageRatingAsync();
    Task<IEnumerable<Doctor>> GetTopRatedDoctorsAsync(int count);
}
