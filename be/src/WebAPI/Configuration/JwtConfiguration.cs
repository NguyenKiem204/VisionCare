using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Configuration;

public static class JwtConfiguration
{
    public static void ConfigureJwtBearer(JwtBearerOptions options, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");

        // Token validation
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Secret"]!)
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
        };

        // Custom events
        options.Events = new JwtBearerEvents { OnChallenge = HandleUnauthorized };
    }

    private static async Task HandleUnauthorized(JwtBearerChallengeContext context)
    {
        context.HandleResponse();

        await ErrorResponseHelper.WriteErrorResponseAsync(
            context.Response,
            "Unauthorized. Please provide a valid access token.",
            401,
            context.Request.Path
        );
    }
}
