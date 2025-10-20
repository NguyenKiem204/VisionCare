using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.CustomerDto;
using VisionCare.Application.Interfaces.Customers;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerByAccountId(int accountId)
    {
        var customer = await _customerService.GetCustomerByAccountIdAsync(accountId);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerRequest request)
    {
        var customer = await _customerService.CreateCustomerAsync(request);
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(
        int id,
        UpdateCustomerRequest request
    )
    {
        var customer = await _customerService.UpdateCustomerAsync(id, request);
        return Ok(customer);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> SearchCustomers(
        [FromQuery] string? keyword,
        [FromQuery] string? gender,
        [FromQuery] DateOnly? fromDob,
        [FromQuery] DateOnly? toDob
    )
    {
        var customers = await _customerService.SearchCustomersAsync(
            keyword,
            gender,
            fromDob,
            toDob
        );
        return Ok(customers);
    }

    [HttpGet("gender/{gender}")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersByGender(string gender)
    {
        var customers = await _customerService.GetCustomersByGenderAsync(gender);
        return Ok(customers);
    }

    [HttpGet("age-range")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersByAgeRange(
        [FromQuery] int minAge,
        [FromQuery] int maxAge
    )
    {
        var customers = await _customerService.GetCustomersByAgeRangeAsync(minAge, maxAge);
        return Ok(customers);
    }

    [HttpPut("{id}/profile")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomerProfile(
        int id,
        UpdateCustomerProfileRequest request
    )
    {
        var customer = await _customerService.UpdateCustomerProfileAsync(id, request);
        return Ok(customer);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult<object>> GetCustomerStatistics()
    {
        var totalCount = await _customerService.GetTotalCustomersCountAsync();
        var genderStats = await _customerService.GetCustomersByGenderStatsAsync();
        var ageGroupStats = await _customerService.GetCustomersByAgeGroupStatsAsync();

        return Ok(
            new
            {
                TotalCount = totalCount,
                GenderStats = genderStats,
                AgeGroupStats = ageGroupStats,
            }
        );
    }
}
