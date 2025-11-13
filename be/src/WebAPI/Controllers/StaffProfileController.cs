using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.StaffDto;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Staff;
using VisionCare.WebAPI.Responses;
using VisionCare.WebAPI.Utils;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/staff/me")]
[Authorize(Policy = "StaffOnly")]
public class StaffProfileController : ControllerBase
{
    private readonly IStaffService _staffService;
    private readonly IS3StorageService _storage;

    public StaffProfileController(IStaffService staffService, IS3StorageService storage)
    {
        _staffService = staffService;
        _storage = storage;
    }

    private int GetCurrentAccountId()
    {
        var idClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("account_id")?.Value
            ?? User.FindFirst("sub")?.Value;
        if (!int.TryParse(idClaim, out var accountId))
        {
            throw new UnauthorizedAccessException("Invalid or missing account id claim.");
        }
        return accountId;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<StaffDto>> GetMyProfile()
    {
        var accountId = GetCurrentAccountId();
        var staff = await _staffService.GetStaffByAccountIdAsync(accountId);
        if (staff == null)
        {
            return NotFound(ApiResponse<StaffDto>.Fail("Staff profile not found."));
        }
        return Ok(ApiResponse<StaffDto>.Ok(staff));
    }

    [HttpPut("profile")]
    public async Task<ActionResult<StaffDto>> UpdateMyProfile(
        [FromForm] UpdateStaffRequest request,
        IFormFile? avatar
    )
    {
        var accountId = GetCurrentAccountId();
        var currentStaff = await _staffService.GetStaffByAccountIdAsync(accountId);
        if (currentStaff == null)
        {
            return NotFound(ApiResponse<StaffDto>.Fail("Staff profile not found."));
        }

        if (avatar != null && avatar.Length > 0)
        {
            var oldKey = S3KeyHelper.TryExtractObjectKey(currentStaff.Avatar);
            if (!string.IsNullOrWhiteSpace(oldKey))
            {
                await _storage.DeleteAsync(oldKey);
            }

            var url = await _storage.UploadAsync(
                avatar.OpenReadStream(),
                avatar.FileName,
                avatar.ContentType,
                UploadPrefixes.StaffAvatar(accountId)
            );
            request.Avatar = url;
        }

        var updated = await _staffService.UpdateStaffAsync(currentStaff.Id, request);
        return Ok(ApiResponse<StaffDto>.Ok(updated));
    }
}
