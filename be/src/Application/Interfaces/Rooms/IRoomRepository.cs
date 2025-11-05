using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Rooms;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAllAsync();
    Task<Room?> GetByIdAsync(int id);
    Task<Room?> GetByCodeAsync(string roomCode);
    Task<Room> AddAsync(Room room);
    Task UpdateAsync(Room room);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByCodeAsync(string roomCode);
    Task<IEnumerable<Room>> GetActiveRoomsAsync();
    Task<(IEnumerable<Room> items, int totalCount)> SearchAsync(
        string? keyword,
        string? status,
        string? location,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    );
}

