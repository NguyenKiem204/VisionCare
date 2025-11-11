namespace VisionCare.Application.DTOs.RoomDto;

public class RoomDto
{
    public int Id { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string? RoomCode { get; set; }
    public int Capacity { get; set; } = 1;
    public string Status { get; set; } = "Active";
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateRoomRequest
{
    public string RoomName { get; set; } = string.Empty;
    public string? RoomCode { get; set; }
    public int Capacity { get; set; } = 1;
    public string Status { get; set; } = "Active";
    public string? Location { get; set; }
    public string? Notes { get; set; }
}

public class UpdateRoomRequest
{
    public string RoomName { get; set; } = string.Empty;
    public string? RoomCode { get; set; }
    public int Capacity { get; set; } = 1;
    public string Status { get; set; } = "Active";
    public string? Location { get; set; }
    public string? Notes { get; set; }
}

public class RoomSearchRequest
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public string? Location { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}

