using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task<Customer?> GetByAccountIdAsync(int accountId);
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);

    // Additional operations for CustomerManager
    Task<IEnumerable<Customer>> SearchCustomersAsync(
        string keyword,
        string? gender,
        DateOnly? fromDob,
        DateOnly? toDob
    );
    Task<IEnumerable<Customer>> GetByGenderAsync(string gender);
    Task<IEnumerable<Customer>> GetByAgeRangeAsync(int minAge, int maxAge);
    Task<int> GetTotalCountAsync();
    Task<Dictionary<string, int>> GetGenderStatsAsync();
    Task<Dictionary<int, int>> GetAgeGroupStatsAsync();
}
