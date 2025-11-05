using System.Text.Json.Serialization;

namespace VisionCare.WebAPI.Responses;

public class ApiResponse<T>
{
    public bool Success { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Errors { get; set; }

    public static ApiResponse<T> Ok(T? data, string? message = null) =>
        new()
        {
            Success = true,
            Message = message,
            Data = data,
        };

    public static ApiResponse<T> Fail(string message, object? errors = null) =>
        new()
        {
            Success = false,
            Message = message,
            Errors = errors,
        };
}

public static class ApiResponse
{
    public static ApiResponse<T> Success<T>(T? data, string? message = null) =>
        ApiResponse<T>.Ok(data, message);
}

public class PagedResponse<TItem>
{
    public bool Success { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
    public IEnumerable<TItem> Items { get; set; } = Array.Empty<TItem>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

    public static PagedResponse<TItem> Ok(
        IEnumerable<TItem> items,
        int totalCount,
        int page,
        int pageSize,
        string? message = null
    ) =>
        new()
        {
            Success = true,
            Message = message,
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        };
}
