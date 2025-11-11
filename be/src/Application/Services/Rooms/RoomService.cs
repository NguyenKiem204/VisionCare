using AutoMapper;
using VisionCare.Application.DTOs.RoomDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.Rooms;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Rooms;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public RoomService(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoomDto>>(rooms);
    }

    public async Task<RoomDto?> GetRoomByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        return room != null ? _mapper.Map<RoomDto>(room) : null;
    }

    public async Task<RoomDto?> GetRoomByCodeAsync(string roomCode)
    {
        var room = await _roomRepository.GetByCodeAsync(roomCode);
        return room != null ? _mapper.Map<RoomDto>(room) : null;
    }

    public async Task<RoomDto> CreateRoomAsync(CreateRoomRequest request)
    {
        // Check if room code already exists
        if (!string.IsNullOrEmpty(request.RoomCode))
        {
            var existingRoom = await _roomRepository.GetByCodeAsync(request.RoomCode);
            if (existingRoom != null)
            {
                throw new ValidationException($"Room with code '{request.RoomCode}' already exists.");
            }
        }

        var room = _mapper.Map<Room>(request);
        room.Created = DateTime.UtcNow;
        var createdRoom = await _roomRepository.AddAsync(room);
        return _mapper.Map<RoomDto>(createdRoom);
    }

    public async Task<RoomDto> UpdateRoomAsync(int id, UpdateRoomRequest request)
    {
        var existingRoom = await _roomRepository.GetByIdAsync(id);
        if (existingRoom == null)
        {
            throw new NotFoundException($"Room with ID {id} not found.");
        }

        // Check if room code already exists (excluding current room)
        if (!string.IsNullOrEmpty(request.RoomCode) && request.RoomCode != existingRoom.RoomCode)
        {
            var roomWithCode = await _roomRepository.GetByCodeAsync(request.RoomCode);
            if (roomWithCode != null && roomWithCode.Id != id)
            {
                throw new ValidationException($"Room with code '{request.RoomCode}' already exists.");
            }
        }

        _mapper.Map(request, existingRoom);
        existingRoom.LastModified = DateTime.UtcNow;
        await _roomRepository.UpdateAsync(existingRoom);
        return _mapper.Map<RoomDto>(existingRoom);
    }

    public async Task<bool> DeleteRoomAsync(int id)
    {
        var existingRoom = await _roomRepository.GetByIdAsync(id);
        if (existingRoom == null)
        {
            return false;
        }

        await _roomRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<RoomDto>> GetActiveRoomsAsync()
    {
        var rooms = await _roomRepository.GetActiveRoomsAsync();
        return _mapper.Map<IEnumerable<RoomDto>>(rooms);
    }

    public async Task<(IEnumerable<RoomDto> items, int totalCount)> SearchRoomsAsync(RoomSearchRequest request)
    {
        var result = await _roomRepository.SearchAsync(
            request.Keyword,
            request.Status,
            request.Location,
            request.Page,
            request.PageSize,
            request.SortBy,
            request.Desc
        );
        return (_mapper.Map<IEnumerable<RoomDto>>(result.items), result.totalCount);
    }
}

