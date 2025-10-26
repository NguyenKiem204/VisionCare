using AutoMapper;
using VisionCare.Application.DTOs.CustomerDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Customers;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Customers;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CustomerService(
        ICustomerRepository customerRepository,
        IUserRepository userRepository,
        IMapper mapper
    )
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<CustomerDto?> GetCustomerByAccountIdAsync(int accountId)
    {
        var customer = await _customerRepository.GetByAccountIdAsync(accountId);
        return customer != null ? _mapper.Map<CustomerDto>(customer) : null;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(request.AccountId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.AccountId} not found.");
        }

        // Check if user already has a customer profile
        if (user.Customer != null)
        {
            throw new ValidationException("User already has a customer profile.");
        }

        // Use AutoMapper to create entity from DTO
        var customer = _mapper.Map<Customer>(request);
        customer.Created = DateTime.UtcNow;

        var createdCustomer = await _customerRepository.AddAsync(customer);
        return _mapper.Map<CustomerDto>(createdCustomer);
    }

    public async Task<CustomerDto> UpdateCustomerAsync(int id, UpdateCustomerRequest request)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(id);
        if (existingCustomer == null)
        {
            throw new NotFoundException($"Customer with ID {id} not found.");
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingCustomer);

        await _customerRepository.UpdateAsync(existingCustomer);
        return _mapper.Map<CustomerDto>(existingCustomer);
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(id);
        if (existingCustomer == null)
        {
            return false;
        }

        await _customerRepository.DeleteAsync(id);
        return true;
    }

    public async Task<(IEnumerable<CustomerDto> items, int totalCount)> SearchCustomersAsync(
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
        var result = await _customerRepository.SearchCustomersAsync(
            keyword,
            gender,
            fromDob,
            toDob,
            page,
            pageSize,
            sortBy,
            desc
        );
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(result.items);
        return (customerDtos, result.totalCount);
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersByGenderAsync(string gender)
    {
        var customers = await _customerRepository.GetByGenderAsync(gender);
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersByAgeRangeAsync(int minAge, int maxAge)
    {
        var customers = await _customerRepository.GetByAgeRangeAsync(minAge, maxAge);
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<CustomerDto> UpdateCustomerProfileAsync(
        int customerId,
        UpdateCustomerProfileRequest request
    )
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with ID {customerId} not found.");
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, customer);

        await _customerRepository.UpdateAsync(customer);
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<int> GetTotalCustomersCountAsync()
    {
        return await _customerRepository.GetTotalCountAsync();
    }

    public async Task<Dictionary<string, int>> GetCustomersByGenderStatsAsync()
    {
        return await _customerRepository.GetGenderStatsAsync();
    }

    public async Task<Dictionary<int, int>> GetCustomersByAgeGroupStatsAsync()
    {
        return await _customerRepository.GetAgeGroupStatsAsync();
    }
}
