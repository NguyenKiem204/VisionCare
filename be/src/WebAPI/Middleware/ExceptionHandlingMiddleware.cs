using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.Exceptions;

namespace VisionCare.WebAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Resource not found: {Message}", ex.Message);
            await HandleNotFoundExceptionAsync(context, ex);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error: {Message}", ex.Message);
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access: {Message}", ex.Message);
            await HandleUnauthorizedExceptionAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument: {Message}", ex.Message);
            await HandleBadRequestExceptionAsync(context, ex);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation: {Message}", ex.Message);
            await HandleBadRequestExceptionAsync(context, ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Request timeout: {Message}", ex.Message);
            await HandleTimeoutExceptionAsync(context, ex);
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Business logic error: {Message}", ex.Message);
            await HandleBusinessExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static async Task HandleNotFoundExceptionAsync(
        HttpContext context,
        NotFoundException ex
    )
    {
        context.Response.StatusCode = 404;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = ex.Message,
            statusCode = 404,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleValidationExceptionAsync(
        HttpContext context,
        ValidationException ex
    )
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = ex.Message,
            statusCode = 400,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "An internal server error occurred. Please try again later.",
            statusCode = 500,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleUnauthorizedExceptionAsync(
        HttpContext context,
        UnauthorizedAccessException ex
    )
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = ex.Message,
            statusCode = 401,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleBadRequestExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = ex.Message,
            statusCode = 400,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleTimeoutExceptionAsync(HttpContext context, TimeoutException ex)
    {
        context.Response.StatusCode = 408;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Request timeout. Please try again.",
            statusCode = 408,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static async Task HandleBusinessExceptionAsync(
        HttpContext context,
        BusinessException ex
    )
    {
        context.Response.StatusCode = ex.StatusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = ex.Message,
            statusCode = ex.StatusCode,
            timestamp = DateTime.UtcNow,
            path = context.Request.Path,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
