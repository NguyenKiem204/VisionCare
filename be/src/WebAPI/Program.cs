using VisionCare.Application;
using VisionCare.Infrastructure;
using VisionCare.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Application services
builder.Services.AddApplication();

// Add Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Add WebAPI services (includes Swagger, FluentValidation, etc.)
builder.Services.AddWebAPIServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add custom middleware
app.UseWebAPIMiddleware();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
