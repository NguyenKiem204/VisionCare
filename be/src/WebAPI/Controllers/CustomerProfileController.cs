using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VisionCare.Application.DTOs.CustomerDto;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Customers;
using VisionCare.WebAPI.Responses;
using VisionCare.WebAPI.Utils;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/customer/me")]
[Authorize]
public class CustomerProfileController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IS3StorageService _storage;

    public CustomerProfileController(ICustomerService customerService, IS3StorageService storage)
    {
        _customerService = customerService;
        _storage = storage;
    }

    private int GetCurrentAccountId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("account_id")?.Value
            ?? User.FindFirst("sub")?.Value;
        if (!int.TryParse(idClaim, out var accountId))
        {
            throw new UnauthorizedAccessException("Invalid or missing account id claim.");
        }
        return accountId;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<CustomerDto>> GetMyProfile()
    {
        var accountId = GetCurrentAccountId();
        var customer = await _customerService.GetCustomerByAccountIdAsync(accountId);
        if (customer == null)
        {
            return NotFound(ApiResponse<CustomerDto>.Fail("Customer profile not found."));
        }
        return Ok(ApiResponse<CustomerDto>.Ok(customer));
    }

    [HttpPut("profile")]
    public async Task<ActionResult<CustomerDto>> UpdateMyProfile(
        [FromForm] UpdateCustomerRequest request,
        IFormFile? avatar
    )
    {
        var accountId = GetCurrentAccountId();
        var currentCustomer = await _customerService.GetCustomerByAccountIdAsync(accountId);
        if (currentCustomer == null)
        {
            return NotFound(ApiResponse<CustomerDto>.Fail("Customer profile not found."));
        }

        if (avatar != null && avatar.Length > 0)
        {
            var oldKey = S3KeyHelper.TryExtractObjectKey(currentCustomer.Avatar);
            if (!string.IsNullOrWhiteSpace(oldKey))
            {
                await _storage.DeleteAsync(oldKey);
            }

            var url = await _storage.UploadAsync(
                avatar.OpenReadStream(),
                avatar.FileName,
                avatar.ContentType,
                UploadPrefixes.CustomerAvatar(accountId)
            );
            request.Avatar = url;
        }

        var updated = await _customerService.UpdateCustomerAsync(currentCustomer.Id, request);
        return Ok(ApiResponse<CustomerDto>.Ok(updated));
    }
}

