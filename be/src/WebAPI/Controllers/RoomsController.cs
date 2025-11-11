using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.RoomDto;
using VisionCare.Application.Interfaces.Rooms;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "StaffOrAdmin")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRooms()
    {
        var rooms = await _roomService.GetAllRoomsAsync();
        return Ok(ApiResponse<IEnumerable<RoomDto>>.Ok(rooms));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveRooms()
    {
        var rooms = await _roomService.GetActiveRoomsAsync();
        return Ok(ApiResponse<IEnumerable<RoomDto>>.Ok(rooms));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoomById(int id)
    {
        var room = await _roomService.GetRoomByIdAsync(id);
        if (room == null)
        {
            return NotFound(ApiResponse<RoomDto>.Fail($"Room with ID {id} not found."));
        }
        return Ok(ApiResponse<RoomDto>.Ok(room));
    }

    [HttpGet("code/{roomCode}")]
    public async Task<IActionResult> GetRoomByCode(string roomCode)
    {
        var room = await _roomService.GetRoomByCodeAsync(roomCode);
        if (room == null)
        {
            return NotFound(ApiResponse<RoomDto>.Fail($"Room with code {roomCode} not found."));
        }
        return Ok(ApiResponse<RoomDto>.Ok(room));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchRooms(
        [FromQuery] string? keyword,
        [FromQuery] string? status,
        [FromQuery] string? location,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool desc = false
    )
    {
        var request = new RoomSearchRequest
        {
            Keyword = keyword,
            Status = status,
            Location = location,
            Page = page,
            PageSize = pageSize,
            SortBy = sortBy,
            Desc = desc
        };
        var result = await _roomService.SearchRoomsAsync(request);
        return Ok(PagedResponse<RoomDto>.Ok(result.items, result.totalCount, page, pageSize));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
    {
        var room = await _roomService.CreateRoomAsync(request);
        return CreatedAtAction(
            nameof(GetRoomById),
            new { id = room.Id },
            ApiResponse<RoomDto>.Ok(room)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomRequest request)
    {
        var room = await _roomService.UpdateRoomAsync(id, request);
        return Ok(ApiResponse<RoomDto>.Ok(room));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var result = await _roomService.DeleteRoomAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<RoomDto>.Fail($"Room with ID {id} not found."));
        }
        return Ok(ApiResponse<RoomDto>.Ok(null, "Room deleted successfully"));
    }
}

