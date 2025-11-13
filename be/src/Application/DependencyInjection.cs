using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.Application.Interfaces.Auth;
using VisionCare.Application.Interfaces.Banners;
using VisionCare.Application.Interfaces.Blogs;
using VisionCare.Application.Interfaces.Content;
using VisionCare.Application.Interfaces.Customers;
using VisionCare.Application.Interfaces.DoctorCertificates;
using VisionCare.Application.Interfaces.DoctorDegrees;
using VisionCare.Application.Interfaces.Doctors;
using VisionCare.Application.Interfaces.Equipment;
using VisionCare.Application.Interfaces.Feedback;
using VisionCare.Application.Interfaces.FollowUp;
using VisionCare.Application.Interfaces.MedicalHistory;
using VisionCare.Application.Interfaces.Reporting;
using VisionCare.Application.Interfaces.Roles;
using VisionCare.Application.Interfaces.Rooms;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Application.Interfaces.ServiceTypes;
using VisionCare.Application.Interfaces.Specializations;
using VisionCare.Application.Interfaces.Staff;
using VisionCare.Application.Interfaces.Users;
using VisionCare.Application.Interfaces.WorkShifts;
using VisionCare.Application.Services.Appointments;
using VisionCare.Application.Services.Banners;
using VisionCare.Application.Services.Blogs;
using VisionCare.Application.Services.Content;
using VisionCare.Application.Services.Customers;
using VisionCare.Application.Services.Dashboard;
using VisionCare.Application.Services.DoctorCertificates;
using VisionCare.Application.Services.DoctorDegrees;
using VisionCare.Application.Services.Doctors;
using VisionCare.Application.Services.Equipment;
using VisionCare.Application.Services.Feedback;
using VisionCare.Application.Services.FollowUp;
using VisionCare.Application.Services.MedicalHistory;
using VisionCare.Application.Services.Roles;
using VisionCare.Application.Services.Rooms;
using VisionCare.Application.Services.Scheduling;
using VisionCare.Application.Services.Services;
using VisionCare.Application.Services.ServiceTypes;
using VisionCare.Application.Services.Specializations;
using VisionCare.Application.Services.Staff;
using VisionCare.Application.Services.Users;
using VisionCare.Application.Services.WorkShifts;

namespace VisionCare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<ISpecializationService, SpecializationService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IServiceDetailService, ServiceDetailService>();

        services.AddScoped<IMedicalHistoryService, MedicalHistoryService>();

        services.AddScoped<IScheduleService, ScheduleService>();

        services.AddScoped<IFeedbackService, FeedbackService>();

        services.AddScoped<IEquipmentService, EquipmentService>();

        services.AddScoped<IServiceTypeService, ServiceTypeService>();

        services.AddScoped<IFollowUpService, FollowUpService>();

        services.AddMemoryCache();
        services.AddScoped<IDashboardService, DashboardService>();

        services.AddScoped<ISectionContentService, SectionContentService>();
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<ICommentBlogService, CommentBlogService>();

        services.AddScoped<IDoctorCertificateService, DoctorCertificateService>();

        services.AddScoped<IDoctorDegreeService, DoctorDegreeService>();

        services.AddScoped<IWeeklyScheduleService, WeeklyScheduleService>();
        services.AddScoped<IScheduleGenerationService, ScheduleGenerationService>();
        services.AddScoped<IDoctorAbsenceService, DoctorAbsenceService>();
        services.AddScoped<ISubstituteDoctorService, SubstituteDoctorService>();
        services.AddScoped<ISlotGenerationService, SlotGenerationService>();
        services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();

        services.AddScoped<VisionCare.Application.Services.Scheduling.ScheduleGenerationJob>();

        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IWorkShiftService, WorkShiftService>();

        return services;
    }
}
