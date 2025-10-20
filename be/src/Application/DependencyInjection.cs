using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.Application.Interfaces.Auth;
using VisionCare.Application.Interfaces.Customers;
using VisionCare.Application.Interfaces.Doctors;
using VisionCare.Application.Interfaces.Feedback;
using VisionCare.Application.Interfaces.MedicalHistory;
using VisionCare.Application.Interfaces.Reporting;
using VisionCare.Application.Interfaces.Roles;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Application.Interfaces.Specializations;
using VisionCare.Application.Interfaces.Staff;
using VisionCare.Application.Interfaces.Users;
using VisionCare.Application.Services.Appointments;
using VisionCare.Application.Services.Customers;
using VisionCare.Application.Services.Dashboard;
using VisionCare.Application.Services.Doctors;
using VisionCare.Application.Services.Feedback;
using VisionCare.Application.Services.MedicalHistory;
using VisionCare.Application.Services.Roles;
using VisionCare.Application.Services.Scheduling;
using VisionCare.Application.Services.Services;
using VisionCare.Application.Services.Specializations;
using VisionCare.Application.Services.Staff;
using VisionCare.Application.Services.Users;

namespace VisionCare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register Core Services
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<ISpecializationService, SpecializationService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        // IAuthService is provided by Infrastructure (real JWT implementation)

        // Register Services Management
        services.AddScoped<IServiceService, ServiceService>();

        // Register Medical History Services
        services.AddScoped<IMedicalHistoryService, MedicalHistoryService>();

        // Register Scheduling Services
        services.AddScoped<IScheduleService, ScheduleService>();

        // Register Feedback Services
        services.AddScoped<IFeedbackService, FeedbackService>();

        // Dashboard & Reporting
        services.AddMemoryCache();
        services.AddScoped<IDashboardService, DashboardService>();

        return services;
    }
}
