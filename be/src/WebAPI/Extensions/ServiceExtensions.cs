using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using VisionCare.WebAPI.Filters;
using VisionCare.WebAPI.Middleware;

namespace VisionCare.WebAPI.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        // Add Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "VisionCare API",
                    Version = "v1",
                    Description = "VisionCare API for managing users, doctors, and appointments",
                }
            );
        });

        // Add FluentValidation
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        // Add custom filters
        services.AddScoped<ValidationFilter>();

        // Add custom services
        services.AddScoped<
            ILogger<ExceptionHandlingMiddleware>,
            Logger<ExceptionHandlingMiddleware>
        >();

        return services;
    }

    public static IApplicationBuilder UseWebAPIMiddleware(this IApplicationBuilder app)
    {
        // Add custom middleware
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}
