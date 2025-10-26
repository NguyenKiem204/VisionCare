using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly VisionCareDbContext _context;

    public CustomerRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var customers = await _context.Customers.Include(c => c.Account).ToListAsync();

        return customers.Select(CustomerMapper.ToDomain);
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        var customer = await _context
            .Customers.Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.AccountId == id || c.Account.AccountId == id);

        return customer != null ? CustomerMapper.ToDomain(customer) : null;
    }

    public async Task<Customer?> GetByAccountIdAsync(int accountId)
    {
        var customer = await _context
            .Customers.Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.AccountId == accountId);

        return customer != null ? CustomerMapper.ToDomain(customer) : null;
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        var customerModel = CustomerMapper.ToInfrastructure(customer);
        _context.Customers.Add(customerModel);
        await _context.SaveChangesAsync();
        return CustomerMapper.ToDomain(customerModel);
    }

    public async Task UpdateAsync(Customer customer)
    {
        var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c =>
            c.AccountId == customer.Id
        );

        if (existingCustomer != null)
        {
            existingCustomer.FullName = customer.CustomerName ?? string.Empty;
            existingCustomer.Gender = customer.Gender;
            existingCustomer.Dob = customer.Dob;
            existingCustomer.Address = customer.Address;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c =>
            c.AccountId == id || c.AccountId == id
        );
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(IEnumerable<Customer> items, int totalCount)> SearchCustomersAsync(
        string keyword,
        string? gender,
        DateOnly? fromDob,
        DateOnly? toDob,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var query = _context.Customers.Include(c => c.Account).AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            var loweredKeyword = keyword.Trim().ToLower();
            query = query.Where(c => 
                c.FullName.ToLower().Contains(loweredKeyword) ||
                c.Account.Email.ToLower().Contains(loweredKeyword) ||
                (c.Phone != null && c.Phone.ToLower().Contains(loweredKeyword))
            );
        }

        if (!string.IsNullOrEmpty(gender))
        {
            query = query.Where(c => c.Gender == gender);
        }

        if (fromDob.HasValue)
        {
            query = query.Where(c => c.Dob >= fromDob.Value);
        }

        if (toDob.HasValue)
        {
            query = query.Where(c => c.Dob <= toDob.Value);
        }

        // Sorting
        query = (sortBy?.ToLower()) switch
        {
            "fullname" => desc ? query.OrderByDescending(c => c.FullName) : query.OrderBy(c => c.FullName),
            "email" => desc ? query.OrderByDescending(c => c.Account.Email) : query.OrderBy(c => c.Account.Email),
            "gender" => desc ? query.OrderByDescending(c => c.Gender) : query.OrderBy(c => c.Gender),
            "dateofbirth" => desc ? query.OrderByDescending(c => c.Dob) : query.OrderBy(c => c.Dob),
            _ => desc ? query.OrderByDescending(c => c.AccountId) : query.OrderBy(c => c.AccountId),
        };

        var totalCount = await query.CountAsync();

        var skip = (Math.Max(page, 1) - 1) * Math.Max(pageSize, 1);
        var customers = await query.Skip(skip).Take(Math.Max(pageSize, 1)).ToListAsync();
        
        return (customers.Select(CustomerMapper.ToDomain), totalCount);
    }

    public async Task<IEnumerable<Customer>> GetByGenderAsync(string gender)
    {
        var customers = await _context
            .Customers.Include(c => c.Account)
            .Where(c => c.Gender == gender)
            .ToListAsync();

        return customers.Select(CustomerMapper.ToDomain);
    }

    public async Task<IEnumerable<Customer>> GetByAgeRangeAsync(int minAge, int maxAge)
    {
        var minDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-maxAge));
        var maxDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-minAge));

        var customers = await _context
            .Customers.Include(c => c.Account)
            .Where(c => c.Dob >= minDate && c.Dob <= maxDate)
            .ToListAsync();

        return customers.Select(CustomerMapper.ToDomain);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Customers.CountAsync();
    }

    public async Task<Dictionary<string, int>> GetGenderStatsAsync()
    {
        return await _context
            .Customers.GroupBy(c => c.Gender)
            .ToDictionaryAsync(g => g.Key ?? "Unknown", g => g.Count());
    }

    public async Task<Dictionary<int, int>> GetAgeGroupStatsAsync()
    {
        var customers = await _context.Customers.ToListAsync();
        var ageGroups = customers
            .GroupBy(c => CalculateAgeGroup(c.Dob))
            .ToDictionary(g => g.Key, g => g.Count());

        return ageGroups;
    }

    private static int CalculateAgeGroup(DateOnly? dob)
    {
        if (!dob.HasValue)
            return 0;

        var age = DateTime.Today.Year - dob.Value.Year;
        if (DateTime.Today.DayOfYear < dob.Value.DayOfYear)
            age--;

        return age switch
        {
            < 18 => 0,
            < 30 => 1,
            < 45 => 2,
            < 60 => 3,
            _ => 4,
        };
    }

    // Mapping moved to CustomerMapper
}
