using VisionCare.Domain.Entities;

namespace VisionCare.Application.DTOs.StaffDto;

public class StaffDto
{
    public int Id { get; set; }
    public int? AccountId { get; set; }
    public string? StaffName { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public int? Age { get; set; }
}

public class CreateStaffRequest
{
    public int AccountId { get; set; }
    public string StaffName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
}

public class UpdateStaffRequest
{
    public string StaffName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
}

public class UpdateStaffProfileRequest
{
    public string StaffName { get; set; } = string.Empty;
    public string? Address { get; set; }
}

public class StaffSearchRequest
{
    public string? Keyword { get; set; }
    public string? Gender { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
