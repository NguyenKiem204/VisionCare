using VisionCare.Application.DTOs.RoomDto;

namespace VisionCare.Application.Interfaces.Rooms;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<RoomDto?> GetRoomByIdAsync(int id);
    Task<RoomDto?> GetRoomByCodeAsync(string roomCode);
    Task<RoomDto> CreateRoomAsync(CreateRoomRequest request);
    Task<RoomDto> UpdateRoomAsync(int id, UpdateRoomRequest request);
    Task<bool> DeleteRoomAsync(int id);
    Task<IEnumerable<RoomDto>> GetActiveRoomsAsync();
    Task<(IEnumerable<RoomDto> items, int totalCount)> SearchRoomsAsync(RoomSearchRequest request);
}

