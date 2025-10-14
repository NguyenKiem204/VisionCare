using System.Text.Json;

namespace VisionCare.WebAPI.Responses;

public static class ErrorResponseHelper
{
    public static object CreateErrorResponse(string message, int statusCode, PathString path)
    {
        return new
        {
            message,
            statusCode,
            timestamp = DateTime.UtcNow,
            path,
        };
    }

    public static async Task WriteErrorResponseAsync(
        HttpResponse response,
        string message,
        int statusCode,
        PathString path
    )
    {
        response.StatusCode = statusCode;
        response.ContentType = "application/json";

        var errorResponse = CreateErrorResponse(message, statusCode, path);
        await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
