using VisionCare.Application.DTOs.CustomerDto;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Customers;

public interface ICustomerService
{
    // Basic CRUD operations
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto?> GetCustomerByAccountIdAsync(int accountId);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request);
    Task<CustomerDto> UpdateCustomerAsync(int id, UpdateCustomerRequest request);
    Task<bool> DeleteCustomerAsync(int id);

    // Business operations
    Task<IEnumerable<CustomerDto>> SearchCustomersAsync(
        string keyword,
        string? gender,
        DateOnly? fromDob,
        DateOnly? toDob
    );
    Task<IEnumerable<CustomerDto>> GetCustomersByGenderAsync(string gender);
    Task<IEnumerable<CustomerDto>> GetCustomersByAgeRangeAsync(int minAge, int maxAge);
    Task<CustomerDto> UpdateCustomerProfileAsync(
        int customerId,
        UpdateCustomerProfileRequest request
    );

    // Statistics
    Task<int> GetTotalCustomersCountAsync();
    Task<Dictionary<string, int>> GetCustomersByGenderStatsAsync();
    Task<Dictionary<int, int>> GetCustomersByAgeGroupStatsAsync();
}
