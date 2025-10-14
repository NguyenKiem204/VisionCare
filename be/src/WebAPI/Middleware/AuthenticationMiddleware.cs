using System.Text.Json;

namespace VisionCare.WebAPI.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        // Only modify response if it hasn't been sent yet
        if (!context.Response.HasStarted)
        {
            // Handle various status codes with custom responses
            if (context.Response.StatusCode == 401)
            {
                await HandleUnauthorizedAsync(context);
            }
            else if (context.Response.StatusCode == 403)
            {
                await HandleForbiddenAsync(context);
            }
            else if (context.Response.StatusCode == 404)
            {
                await HandleNotFoundAsync(context);
            }
            else if (context.Response.StatusCode == 405)
            {
                await HandleMethodNotAllowedAsync(context);
            }
            else if (context.Response.StatusCode == 429)
            {
                await HandleTooManyRequestsAsync(context);
            }
        }
    }

    private static async Task HandleUnauthorizedAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Unauthorized. Please provide a valid access token.",
            statusCode = 401,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleForbiddenAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Forbidden. You don't have permission to access this resource.",
            statusCode = 403,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleNotFoundAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Resource not found.",
            statusCode = 404,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleMethodNotAllowedAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Method not allowed for this endpoint.",
            statusCode = 405,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleTooManyRequestsAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Too many requests. Please try again later.",
            statusCode = 429,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
