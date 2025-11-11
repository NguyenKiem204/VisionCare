using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Rooms;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;
using InfrastructureRoom = VisionCare.Infrastructure.Models.Room;

namespace VisionCare.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly VisionCareDbContext _context;

    public RoomRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        var rooms = await _context.Rooms.ToListAsync();
        return rooms.Select(RoomMapper.ToDomain);
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == id);
        return room != null ? RoomMapper.ToDomain(room) : null;
    }

    public async Task<Room?> GetByCodeAsync(string roomCode)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomCode == roomCode);
        return room != null ? RoomMapper.ToDomain(room) : null;
    }

    public async Task<Room> AddAsync(Room room)
    {
        var roomModel = RoomMapper.ToInfrastructure(room);
        _context.Rooms.Add(roomModel);
        await _context.SaveChangesAsync();
        room.Id = roomModel.RoomId;
        return room;
    }

    public async Task UpdateAsync(Room room)
    {
        var existingRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomId == room.Id);
        if (existingRoom != null)
        {
            existingRoom.RoomName = room.RoomName;
            existingRoom.RoomCode = room.RoomCode;
            existingRoom.Capacity = room.Capacity;
            existingRoom.Status = room.Status;
            existingRoom.Location = room.Location;
            existingRoom.Notes = room.Notes;
            existingRoom.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room != null)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Rooms.AnyAsync(r => r.RoomId == id);
    }

    public async Task<bool> ExistsByCodeAsync(string roomCode)
    {
        return await _context.Rooms.AnyAsync(r => r.RoomCode == roomCode);
    }

    public async Task<IEnumerable<Room>> GetActiveRoomsAsync()
    {
        var rooms = await _context.Rooms
            .Where(r => r.Status == "Active")
            .ToListAsync();
        return rooms.Select(RoomMapper.ToDomain);
    }

    public async Task<(IEnumerable<Room> items, int totalCount)> SearchAsync(
        string? keyword,
        string? status,
        string? location,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var query = _context.Rooms.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            var loweredKeyword = keyword.Trim().ToLower();
            query = query.Where(r =>
                r.RoomName.ToLower().Contains(loweredKeyword) ||
                (r.RoomCode != null && r.RoomCode.ToLower().Contains(loweredKeyword)) ||
                (r.Location != null && r.Location.ToLower().Contains(loweredKeyword))
            );
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(r => r.Status == status);
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(r => r.Location != null && r.Location.Contains(location));
        }

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "name" => desc ? query.OrderByDescending(r => r.RoomName) : query.OrderBy(r => r.RoomName),
            "code" => desc ? query.OrderByDescending(r => r.RoomCode) : query.OrderBy(r => r.RoomCode),
            "status" => desc ? query.OrderByDescending(r => r.Status) : query.OrderBy(r => r.Status),
            "location" => desc ? query.OrderByDescending(r => r.Location) : query.OrderBy(r => r.Location),
            _ => desc ? query.OrderByDescending(r => r.RoomId) : query.OrderBy(r => r.RoomId)
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items.Select(RoomMapper.ToDomain), totalCount);
    }
}

