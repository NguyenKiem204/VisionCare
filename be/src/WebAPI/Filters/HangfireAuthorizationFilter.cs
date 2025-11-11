using Hangfire.Dashboard;

namespace VisionCare.WebAPI.Filters;

/// <summary>
/// Authorization filter for Hangfire Dashboard
/// Only allows Admin users to access the dashboard
/// </summary>
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // In production, you should check if the user is authenticated and has Admin role
        // For now, allow in Development, require authentication in Production
        var httpContext = context.GetHttpContext();
        
        if (httpContext == null)
            return false;

        // Allow in Development environment
        var environment = httpContext.RequestServices.GetRequiredService<Microsoft.Extensions.Hosting.IHostEnvironment>();
        if (environment.IsDevelopment())
        {
            return true; // Allow access in development
        }

        // In Production: Check if user is authenticated and has Admin role
        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
        {
            return false;
        }

        return httpContext.User.IsInRole("Admin");
    }
}

