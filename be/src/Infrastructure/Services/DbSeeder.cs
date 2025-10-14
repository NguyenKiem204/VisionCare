using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Models;

namespace VisionCare.Infrastructure.Services;

public static class DbSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VisionCareDbContext>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<object>>();

        var isEnabled = bool.Parse(config["AdminSeed:Enabled"] ?? "false");
        if (!isEnabled)
        {
            logger.LogInformation("Admin seeding is disabled in configuration.");
            return;
        }

        try
        {
            var username = config["AdminSeed:Username"] ?? "admin";
            var email = config["AdminSeed:Email"] ?? "admin@visioncare.com";
            var password = config["AdminSeed:Password"] ?? "admin123";
            var roleName = config["AdminSeed:Role"] ?? "Admin";

            var adminRole = await db.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (adminRole == null)
            {
                adminRole = new Role { RoleName = roleName, RoleDescription = "Administrator" };
                db.Roles.Add(adminRole);
                await db.SaveChangesAsync();
                logger.LogInformation("Created {RoleName} role.", roleName);
            }

            var admin = await db.Accounts.FirstOrDefaultAsync(a => a.Username == username);
            if (admin == null)
            {
                admin = new Account
                {
                    Email = email,
                    Username = username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    EmailConfirmed = true,
                    Status = "Active",
                    RoleId = adminRole.RoleId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                db.Accounts.Add(admin);
                await db.SaveChangesAsync();
                logger.LogWarning(
                    "Admin user created with username: {Username}, email: {Email}. Please change the default password!",
                    username,
                    email
                );
            }
            else
            {
                logger.LogInformation(
                    "Admin user already exists with username: {Username}",
                    username
                );
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding admin user.");
        }
    }
}
