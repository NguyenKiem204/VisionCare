using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VisionCare.Application.DTOs.Common;
using VisionCare.Application.DTOs.CustomerHistory;
using VisionCare.Application.Interfaces.CustomerHistory;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/customer/me")]
[Authorize]
public class CustomerHistoryController : ControllerBase
{
    private readonly ICustomerHistoryService _historyService;

    public CustomerHistoryController(ICustomerHistoryService historyService)
    {
        _historyService = historyService;
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

    [HttpGet("bookings")]
    public async Task<ActionResult<ApiResponse<PagedResult<CustomerBookingHistoryDto>>>> GetBookings(
        [FromQuery] CustomerBookingHistoryQuery query
    )
    {
        var accountId = GetCurrentAccountId();
        var result = await _historyService.GetBookingsAsync(accountId, query);
        return Ok(ApiResponse<PagedResult<CustomerBookingHistoryDto>>.Ok(result));
    }

    [HttpGet("prescriptions")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerPrescriptionHistoryDto>>>> GetPrescriptions(
        [FromQuery] int? encounterId = null
    )
    {
        var accountId = GetCurrentAccountId();
        var result = await _historyService.GetPrescriptionsAsync(accountId, encounterId);
        return Ok(ApiResponse<IEnumerable<CustomerPrescriptionHistoryDto>>.Ok(result));
    }

    [HttpGet("medical-history")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerMedicalHistoryDto>>>> GetMedicalHistory()
    {
        var accountId = GetCurrentAccountId();
        var result = await _historyService.GetMedicalHistoryAsync(accountId);
        return Ok(ApiResponse<IEnumerable<CustomerMedicalHistoryDto>>.Ok(result));
    }
}

