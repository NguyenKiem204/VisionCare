using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.EquipmentDto;
using VisionCare.Application.Interfaces.Equipment;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAdmin")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;

    public EquipmentController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEquipment()
    {
        var equipment = await _equipmentService.GetAllEquipmentAsync();
        return Ok(ApiResponse<IEnumerable<EquipmentDto>>.Ok(equipment));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEquipmentById(int id)
    {
        var equipment = await _equipmentService.GetEquipmentByIdAsync(id);
        if (equipment == null)
        {
            return NotFound(ApiResponse<EquipmentDto>.Fail($"Equipment with ID {id} not found."));
        }
        return Ok(ApiResponse<EquipmentDto>.Ok(equipment));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchEquipment(
        [FromQuery] string? keyword,
        [FromQuery] string? status,
        [FromQuery] string? location,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool desc = false
    )
    {
        var result = await _equipmentService.SearchEquipmentAsync(
            keyword,
            status,
            location,
            page,
            pageSize,
            sortBy,
            desc
        );
        return Ok(PagedResponse<EquipmentDto>.Ok(result.items, result.totalCount, page, pageSize));
    }

    [HttpPost]
    public async Task<IActionResult> CreateEquipment([FromBody] CreateEquipmentRequest request)
    {
        var equipment = await _equipmentService.CreateEquipmentAsync(request);
        return CreatedAtAction(nameof(GetEquipmentById), new { id = equipment.Id }, ApiResponse<EquipmentDto>.Ok(equipment));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEquipment(int id, [FromBody] UpdateEquipmentRequest request)
    {
        var equipment = await _equipmentService.UpdateEquipmentAsync(id, request);
        return Ok(ApiResponse<EquipmentDto>.Ok(equipment));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipment(int id)
    {
        var result = await _equipmentService.DeleteEquipmentAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<EquipmentDto>.Fail($"Equipment with ID {id} not found."));
        }
        return Ok(ApiResponse<EquipmentDto>.Ok(null, "Equipment deleted successfully"));
    }
}
