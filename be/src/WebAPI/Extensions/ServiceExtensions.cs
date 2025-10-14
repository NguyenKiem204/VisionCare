using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VisionCare.WebAPI.Authorization;
using VisionCare.WebAPI.Configuration;
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

        services.Configure<MvcOptions>(options =>
        {
            options.Filters.AddService<ValidationFilter>(order: int.MinValue);
        });

        // Centralized CORS policy
        services.AddCors(options =>
        {
            options.AddPolicy(
                "DefaultCors",
                policy =>
                    policy
                        .SetIsOriginAllowed(origin =>
                        {
                            if (string.IsNullOrEmpty(origin))
                                return false;

                            if (
                                origin.StartsWith(
                                    "http://localhost:",
                                    StringComparison.OrdinalIgnoreCase
                                )
                            )
                                return true;

                            return false;
                        })
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .AllowCredentials()
            );
        });

        // Authorization policies centralized here
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
            options.AddPolicy("DoctorOnly", p => p.RequireRole("Doctor"));
            options.AddPolicy("StaffOnly", p => p.RequireRole("Staff"));
            options.AddPolicy("CustomerOnly", p => p.RequireRole("Customer"));

            options.AddPolicy("UserOrAdmin", p => p.RequireRole("Customer", "Admin"));
            options.AddPolicy("StaffOrAdmin", p => p.RequireRole("Staff", "Admin"));
            options.AddPolicy("DoctorOrAdmin", p => p.RequireRole("Doctor", "Admin"));
            options.AddPolicy(
                "AnyRole",
                p => p.RequireRole("Admin", "Doctor", "Staff", "Customer")
            );
            options.AddPolicy(
                "OwnProfileOrAdmin",
                p => p.Requirements.Add(new OwnProfileOrAdminRequirement())
            );
        });

        services.AddSingleton<IAuthorizationHandler, OwnProfileOrAdminHandler>();

        return services;
    }

    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => JwtConfiguration.ConfigureJwtBearer(options, configuration));

        return services;
    }

    public static IApplicationBuilder UseWebAPIMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}
