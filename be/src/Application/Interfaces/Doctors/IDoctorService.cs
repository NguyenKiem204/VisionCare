using VisionCare.Application.DTOs.DoctorDto;

namespace VisionCare.Application.Interfaces.Doctors;

/// <summary>
/// Service interface for doctor management operations
/// </summary>
public interface IDoctorService
{
    // Basic CRUD operations
    Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
    Task<DoctorDto?> GetDoctorByIdAsync(int id);
    Task<DoctorDto> CreateDoctorAsync(CreateDoctorRequest request);
    Task<DoctorDto> UpdateDoctorAsync(int id, UpdateDoctorRequest request);
    Task<bool> DeleteDoctorAsync(int id);

    // Business operations
    Task<IEnumerable<DoctorDto>> GetDoctorsBySpecializationAsync(int specializationId);
    Task<IEnumerable<DoctorDto>> GetAvailableDoctorsAsync(DateTime date);
    Task<(IEnumerable<DoctorDto> items, int totalCount)> SearchDoctorsAsync(
        string keyword,
        int? specializationId,
        double? minRating,
        int page = 1,
        int pageSize = 10,
        string sortBy = "id",
        bool desc = false
    );
    Task<DoctorDto> UpdateDoctorRatingAsync(int doctorId, double newRating);
    Task<DoctorDto> UpdateDoctorStatusAsync(int doctorId, string status);

    // Statistics
    Task<int> GetTotalDoctorsCountAsync();
    Task<double> GetAverageRatingAsync();
    Task<IEnumerable<DoctorDto>> GetTopRatedDoctorsAsync(int count = 5);
}