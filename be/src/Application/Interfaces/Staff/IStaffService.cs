using VisionCare.Application.DTOs.StaffDto;

namespace VisionCare.Application.Interfaces.Staff;

/// <summary>
/// Service interface for staff management operations
/// </summary>
public interface IStaffService
{
    // Basic CRUD operations
    Task<IEnumerable<StaffDto>> GetAllStaffAsync();
    Task<StaffDto?> GetStaffByIdAsync(int id);
    Task<StaffDto?> GetStaffByAccountIdAsync(int accountId);
    Task<StaffDto> CreateStaffAsync(CreateStaffRequest request);
    Task<StaffDto> UpdateStaffAsync(int id, UpdateStaffRequest request);
    Task<bool> DeleteStaffAsync(int id);

    // Business operations
    Task<IEnumerable<StaffDto>> SearchStaffAsync(string keyword, string? gender);
    Task<IEnumerable<StaffDto>> GetStaffByGenderAsync(string gender);
    Task<StaffDto> UpdateStaffProfileAsync(int staffId, UpdateStaffProfileRequest request);

    // Statistics
    Task<int> GetTotalStaffCountAsync();
    Task<Dictionary<string, int>> GetStaffByGenderStatsAsync();
}
