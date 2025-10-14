using VisionCare.Application;
using VisionCare.Infrastructure;
using VisionCare.WebAPI.Extensions;
using DbSeeder = VisionCare.Infrastructure.Services.DbSeeder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddWebAPIServices();

builder.Services.AddAuthenticationServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebAPIMiddleware();

app.UseHttpsRedirection();
app.UseCors("DefaultCors");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<VisionCare.WebAPI.Middleware.AuthenticationMiddleware>();
app.MapControllers();

await DbSeeder.SeedAdminAsync(app.Services);

app.Run();
