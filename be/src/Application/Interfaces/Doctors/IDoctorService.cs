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
    Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(
        string keyword,
        int? specializationId,
        double? minRating
    );
    Task<DoctorDto> UpdateDoctorRatingAsync(int doctorId, double newRating);
    Task<DoctorDto> UpdateDoctorStatusAsync(int doctorId, string status);

    // Statistics
    Task<int> GetTotalDoctorsCountAsync();
    Task<double> GetAverageRatingAsync();
    Task<IEnumerable<DoctorDto>> GetTopRatedDoctorsAsync(int count = 5);
}