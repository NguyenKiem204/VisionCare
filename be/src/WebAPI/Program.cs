using Hangfire;
using Hangfire.PostgreSql;
using VisionCare.Application;
using VisionCare.Infrastructure;
using VisionCare.WebAPI.Extensions;
using VisionCare.WebAPI.Filters;
using VisionCare.WebAPI.Hubs;
using DbSeeder = VisionCare.Infrastructure.Services.DbSeeder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddWebAPIServices();

builder.Services.AddAuthenticationServices(builder.Configuration);

// Configure Hangfire
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
#pragma warning disable CS0618
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions
    {
        SchemaName = "hangfire", // Optional: separate schema for Hangfire tables
        QueuePollInterval = TimeSpan.FromSeconds(15),
        InvisibilityTimeout = TimeSpan.FromMinutes(5),
        DistributedLockTimeout = TimeSpan.FromMinutes(5),
        JobExpirationCheckInterval = TimeSpan.FromHours(1),
        CountersAggregateInterval = TimeSpan.FromMinutes(5),
        PrepareSchemaIfNecessary = true,
        EnableTransactionScopeEnlistment = true
    })
);
#pragma warning restore CS0618

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = Environment.ProcessorCount * 5; // Adjust based on your needs
    options.ServerTimeout = TimeSpan.FromMinutes(4);
    options.SchedulePollingInterval = TimeSpan.FromSeconds(15);
});

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

// Hangfire Dashboard (only accessible by Admin)
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() },
    DashboardTitle = "VisionCare Background Jobs",
    StatsPollingInterval = 2000,
    DisplayStorageConnectionString = false
});

// Map SignalR Hub
app.MapHub<BookingHub>("/hubs/booking");

app.MapControllers();

// Seed data
await DbSeeder.SeedAdminAsync(app.Services);

// Configure Hangfire Recurring Jobs
var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Schedule Generation Job - Runs daily at 02:00 AM
// Note: Hangfire will create a new scope and resolve ScheduleGenerationJob from DI
recurringJobManager.AddOrUpdate<VisionCare.Application.Services.Scheduling.ScheduleGenerationJob>(
    "schedule-generation-job",
    job => job.ExecuteAsync(),
    "0 2 * * *", // Cron expression: Every day at 02:00 AM
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    }
);

logger.LogInformation("Hangfire recurring jobs configured successfully");
logger.LogInformation("Schedule Generation Job will run daily at 02:00 AM");

app.Run();
