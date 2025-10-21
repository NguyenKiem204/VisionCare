using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<(IEnumerable<User> items, int totalCount)> SearchAsync(
        string? keyword,
        int? roleId,
        string? status,
        int page,
        int pageSize,
        string? sortBy,
        bool desc
    );
}
