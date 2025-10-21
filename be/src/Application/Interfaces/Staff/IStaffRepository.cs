using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces;

public interface IStaffRepository
{
    Task<IEnumerable<Domain.Entities.Staff>> GetAllAsync();
    Task<Domain.Entities.Staff?> GetByIdAsync(int id);
    Task<Domain.Entities.Staff?> GetByAccountIdAsync(int accountId);
    Task<Domain.Entities.Staff> AddAsync(Domain.Entities.Staff staff);
    Task UpdateAsync(Domain.Entities.Staff staff);
    Task DeleteAsync(int id);

    // Additional operations for StaffManager
    Task<IEnumerable<Domain.Entities.Staff>> SearchAsync(string keyword, string? gender);
    Task<IEnumerable<Domain.Entities.Staff>> GetByGenderAsync(string gender);
    Task<int> GetTotalCountAsync();
    Task<Dictionary<string, int>> GetGenderStatsAsync();
}
