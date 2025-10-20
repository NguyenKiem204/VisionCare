using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.CustomerDto;

public class CustomerDto
{
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public string? CustomerName { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public int? Age { get; set; }
}

public class CreateCustomerRequest
{
    public int AccountId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
}

public class UpdateCustomerRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
}

public class UpdateCustomerProfileRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public string? Address { get; set; }
}

public class CustomerSearchRequest
{
    public string? Keyword { get; set; }
    public string? Gender { get; set; }
    public DateOnly? FromDob { get; set; }
    public DateOnly? ToDob { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
