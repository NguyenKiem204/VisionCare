using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Auth;
using VisionCare.Application.Interfaces.Equipment;
using VisionCare.Application.Interfaces.Feedback;
using VisionCare.Application.Interfaces.FollowUp;
using VisionCare.Application.Interfaces.MedicalHistory;
using VisionCare.Application.Interfaces.Reporting;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Application.Interfaces.ServiceTypes;
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

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<ISpecializationRepository, SpecializationRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IMedicalHistoryRepository, MedicalHistoryRepository>();
        services.AddScoped<ISlotRepository, SlotRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IFeedbackDoctorRepository, FeedbackDoctorRepository>();
        services.AddScoped<IFeedbackServiceRepository, FeedbackServiceRepository>();
        services.AddScoped<IReportingRepository, ReportingRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
        services.AddScoped<IFollowUpRepository, FollowUpRepository>();

        // Cross-cutting infrastructure services
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
