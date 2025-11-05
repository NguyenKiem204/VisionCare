using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Doctors;
using VisionCare.Application.Interfaces.Scheduling;

namespace VisionCare.Application.Services.Scheduling;

/// <summary>
/// Hangfire Recurring Job for automatic schedule generation
/// Runs daily at 02:00 AM
/// </summary>
public class ScheduleGenerationJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScheduleGenerationJob> _logger;

    public ScheduleGenerationJob(
        IServiceProvider serviceProvider,
        ILogger<ScheduleGenerationJob> logger
    )
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Generate schedules for all doctors daily
    /// This method is called by Hangfire RecurringJob
    /// </summary>
    public async Task ExecuteAsync()
    {
        _logger.LogInformation(
            "ScheduleGenerationJob started at {Time}",
            DateTime.Now
        );

        using var scope = _serviceProvider.CreateScope();
        var scheduleGenerationService = scope.ServiceProvider.GetRequiredService<
            IScheduleGenerationService
        >();
        var scheduleRepository = scope.ServiceProvider.GetRequiredService<IScheduleRepository>();
        var doctorRepository = scope.ServiceProvider.GetRequiredService<IDoctorRepository>();

        try
        {
            // Calculate date range: Tomorrow to 30 days ahead
            // StartDate: Tomorrow (Ngày mai)
            // EndDate: 30 days from today
            var tomorrow = DateTime.Today.AddDays(1);
            var startDate = DateOnly.FromDateTime(tomorrow);
            var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));

            _logger.LogInformation(
                "Generating schedules from {StartDate} to {EndDate}",
                startDate,
                endDate
            );

            // Generate schedules for all doctors using DoctorSchedule (new flexible scheduling)
            var doctors = await doctorRepository.GetAllAsync();
            var doctorIds = doctors.Select(d => d.AccountId ?? 0).Where(id => id > 0).Distinct();

            int totalGenerated = 0;
            foreach (var doctorId in doctorIds)
            {
                try
                {
                    var count = await scheduleGenerationService.GenerateSchedulesFromAllDoctorSchedulesAsync(
                        doctorId,
                        30
                    );
                    totalGenerated += count;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Failed to generate schedules for doctor {DoctorId}",
                        doctorId
                    );
                    // Continue with other doctors
                }
            }

            // Remove old WeeklySchedule generation to avoid double-creating

            _logger.LogInformation(
                "Generated {Count} schedules successfully",
                totalGenerated
            );

            // Cleanup old schedules (older than 90 days)
            var cleanedCount = await scheduleRepository.CleanupOldSchedulesAsync(90);
            if (cleanedCount > 0)
            {
                _logger.LogInformation("Cleaned up {Count} old schedules", cleanedCount);
            }

            _logger.LogInformation(
                "ScheduleGenerationJob completed successfully at {Time}",
                DateTime.Now
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in ScheduleGenerationJob");
            throw; // Hangfire will handle retry automatically
        }
    }
}

