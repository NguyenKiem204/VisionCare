using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.StaffDto;
using VisionCare.Application.Interfaces.Staff;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<StaffDto>>> GetStaff()
    {
        var staff = await _staffService.GetAllStaffAsync();
        return Ok(staff);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StaffDto>> GetStaff(int id)
    {
        var staff = await _staffService.GetStaffByIdAsync(id);
        if (staff == null)
        {
            return NotFound();
        }
        return Ok(staff);
    }

    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<StaffDto>> GetStaffByAccountId(int accountId)
    {
        var staff = await _staffService.GetStaffByAccountIdAsync(accountId);
        if (staff == null)
        {
            return NotFound();
        }
        return Ok(staff);
    }

    [HttpPost]
    public async Task<ActionResult<StaffDto>> CreateStaff(CreateStaffRequest request)
    {
        var staff = await _staffService.CreateStaffAsync(request);
        return CreatedAtAction(nameof(GetStaff), new { id = staff.Id }, staff);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StaffDto>> UpdateStaff(int id, UpdateStaffRequest request)
    {
        var staff = await _staffService.UpdateStaffAsync(id, request);
        return Ok(staff);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStaff(int id)
    {
        var result = await _staffService.DeleteStaffAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<StaffDto>>> SearchStaff(
        [FromQuery] string? keyword,
        [FromQuery] string? gender
    )
    {
        var staff = await _staffService.SearchStaffAsync(keyword ?? string.Empty, gender);
        return Ok(staff);
    }

    [HttpGet("gender/{gender}")]
    public async Task<ActionResult<IEnumerable<StaffDto>>> GetStaffByGender(string gender)
    {
        var staff = await _staffService.GetStaffByGenderAsync(gender);
        return Ok(staff);
    }

    [HttpPut("{id}/profile")]
    public async Task<ActionResult<StaffDto>> UpdateStaffProfile(
        int id,
        UpdateStaffProfileRequest request
    )
    {
        var staff = await _staffService.UpdateStaffProfileAsync(id, request);
        return Ok(staff);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult<object>> GetStaffStatistics()
    {
        var totalCount = await _staffService.GetTotalStaffCountAsync();
        var genderStats = await _staffService.GetStaffByGenderStatsAsync();

        return Ok(new { TotalCount = totalCount, GenderStats = genderStats });
    }
}
