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
    Task<(IEnumerable<Doctor> items, int totalCount)> SearchDoctorsAsync(
        string keyword,
        int? specializationId,
        double? minRating,
        int page = 1,
        int pageSize = 10,
        string sortBy = "id",
        bool desc = false
    );
    Task<int> GetTotalCountAsync();
    Task<double> GetAverageRatingAsync();
    Task<IEnumerable<Doctor>> GetTopRatedDoctorsAsync(int count);
}
