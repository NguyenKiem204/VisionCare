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
            existingCustomer.FullName = customer.CustomerName;
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

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(
        string keyword,
        string? gender,
        DateOnly? fromDob,
        DateOnly? toDob
    )
    {
        var query = _context.Customers.Include(c => c.Account).AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(c => c.FullName.Contains(keyword));
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

        var customers = await query.ToListAsync();
        return customers.Select(CustomerMapper.ToDomain);
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
