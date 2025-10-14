using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Auth;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Repositories;
using VisionCare.Infrastructure.Services;

namespace VisionCare.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<VisionCareDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
