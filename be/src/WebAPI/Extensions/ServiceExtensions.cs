using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using VisionCare.WebAPI.Filters;
using VisionCare.WebAPI.Middleware;

namespace VisionCare.WebAPI.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
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

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddScoped<ValidationFilter>();


        return services;
    }

    public static IApplicationBuilder UseWebAPIMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}
