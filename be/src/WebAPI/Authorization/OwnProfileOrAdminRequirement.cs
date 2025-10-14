using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace VisionCare.WebAPI.Authorization;

public class OwnProfileOrAdminRequirement : IAuthorizationRequirement { }

public class OwnProfileOrAdminHandler : AuthorizationHandler<OwnProfileOrAdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnProfileOrAdminRequirement requirement
    )
    {
        // Admin passes
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Check route id equals user id (sub)
        if (
            context.Resource is HttpContext httpContext
            && httpContext.Request.RouteValues.TryGetValue("id", out var idObj)
            && idObj != null
            && int.TryParse(idObj.ToString(), out var routeId)
        )
        {
            var sub =
                context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? context.User.FindFirstValue(ClaimTypes.Name)
                ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (
                int.TryParse(
                    context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "",
                    out var userId
                )
            )
            {
                if (userId == routeId)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
        }

        return Task.CompletedTask;
    }
}
