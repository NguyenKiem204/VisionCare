using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Auth;
using VisionCare.Application.Interfaces.Banners;
using VisionCare.Application.Interfaces.Blogs;
using VisionCare.Application.Interfaces.Booking;
using VisionCare.Application.Interfaces.Content;
using VisionCare.Application.Interfaces.CustomerHistory;
using VisionCare.Application.Interfaces.DoctorCertificates;
using VisionCare.Application.Interfaces.DoctorDegrees;
using VisionCare.Application.Interfaces.Ehr;
using VisionCare.Application.Interfaces.Equipment;
using VisionCare.Application.Interfaces.Feedback;
using VisionCare.Application.Interfaces.FollowUp;
using VisionCare.Application.Interfaces.MedicalHistory;
using VisionCare.Application.Interfaces.Payment;
using VisionCare.Application.Interfaces.Reporting;
using VisionCare.Application.Interfaces.Rooms;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Application.Interfaces.ServiceTypes;
using VisionCare.Application.Interfaces.WorkShifts;
using VisionCare.Application.Services.CustomerHistory;
using VisionCare.Application.Services.Ehr;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Repositories;
using VisionCare.Infrastructure.Repositories.CustomerHistory;
using VisionCare.Infrastructure.Repositories.Ehr;
using VisionCare.Infrastructure.Services;
using VisionCare.Infrastructure.Services.Payment;

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

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IDoctorDegreeRepository, DoctorDegreeRepository>();
        services.AddScoped<IDoctorCertificateRepository, DoctorCertificateRepository>();
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
        services.AddScoped<ISectionContentRepository, SectionContentRepository>();
        services.AddScoped<IBannerRepository, BannerRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<ICommentBlogRepository, CommentBlogRepository>();
        services.AddScoped<ICustomerHistoryReadRepository, CustomerHistoryReadRepository>();

        services.AddScoped<IWeeklyScheduleRepository, WeeklyScheduleRepository>();
        services.AddScoped<IDoctorAbsenceRepository, DoctorAbsenceRepository>();
        services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IWorkShiftRepository, WorkShiftRepository>();

        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IS3StorageService, S3StorageService>();

        var redisConnectionString = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "VisionCare:";
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.AddScoped<IBookingHoldCacheService, BookingHoldCacheService>();

        services.AddScoped<IVNPayService, VNPayService>();

        services.AddScoped<
            VisionCare.Application.Interfaces.Booking.IGuestAccountService,
            VisionCare.Infrastructure.Services.GuestAccountService
        >();

        services.AddScoped<
            VisionCare.Application.Interfaces.Services.IServiceDetailRepository,
            VisionCare.Infrastructure.Repositories.ServiceDetailRepository
        >();

        services.AddScoped<
            VisionCare.Application.Interfaces.IDiscountRepository,
            VisionCare.Infrastructure.Repositories.DiscountRepository
        >();

        services.AddScoped<
            VisionCare.Application.Interfaces.Payment.ICheckoutService,
            VisionCare.Infrastructure.Services.Payment.CheckoutService
        >();

        services.AddScoped<
            VisionCare.Application.Interfaces.Payment.IPaymentConfiguration,
            VisionCare.Infrastructure.Services.Payment.PaymentConfiguration
        >();

        services.AddScoped<
            VisionCare.Application.Interfaces.Booking.IBookingService,
            VisionCare.Application.Services.Booking.BookingService
        >();

        services.AddScoped<IEncounterRepository, EncounterRepository>();
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IEncounterService, EncounterService>();
        services.AddScoped<IPrescriptionService, PrescriptionService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICustomerHistoryService, CustomerHistoryService>();

        return services;
    }
}
