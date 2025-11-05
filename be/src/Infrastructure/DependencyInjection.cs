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
using VisionCare.Application.Interfaces.Rooms;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.WorkShifts;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Application.Interfaces.ServiceTypes;
using VisionCare.Application.Interfaces.Content;
using VisionCare.Application.Interfaces.Banners;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Repositories;
using VisionCare.Infrastructure.Services;
using VisionCare.Application.Interfaces.Booking;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using VisionCare.Application.Interfaces.Payment;
using VisionCare.Infrastructure.Services.Payment;
using VisionCare.Application.Interfaces.Ehr;
using VisionCare.Infrastructure.Repositories.Ehr;
using VisionCare.Application.Services.Ehr;

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
        services.AddScoped<ISectionContentRepository, SectionContentRepository>();
        services.AddScoped<IBannerRepository, BannerRepository>();
        
        // Scheduling Repositories
        services.AddScoped<IWeeklyScheduleRepository, WeeklyScheduleRepository>();
        services.AddScoped<IDoctorAbsenceRepository, DoctorAbsenceRepository>();
        services.AddScoped<IDoctorScheduleRepository, DoctorScheduleRepository>();
        
        // Room & WorkShift Repositories
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IWorkShiftRepository, WorkShiftRepository>();

        // Cross-cutting infrastructure services
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        // Storage
        services.AddScoped<IS3StorageService, S3StorageService>();

        // Redis Configuration cho Booking Hold
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
            // Fallback to in-memory cache nếu không có Redis
            services.AddDistributedMemoryCache();
        }

        // Register BookingHoldCacheService
        services.AddScoped<IBookingHoldCacheService, BookingHoldCacheService>();

        // Register VNPay Service
        services.AddScoped<IVNPayService, VNPayService>();

        // Register Guest Account Service
        services.AddScoped<VisionCare.Application.Interfaces.Booking.IGuestAccountService, 
            VisionCare.Infrastructure.Services.GuestAccountService>();

        // Register Service Detail Repository
        services.AddScoped<VisionCare.Application.Interfaces.Services.IServiceDetailRepository,
            VisionCare.Infrastructure.Repositories.ServiceDetailRepository>();

        // Register Discount Repository
        services.AddScoped<VisionCare.Application.Interfaces.IDiscountRepository,
            VisionCare.Infrastructure.Repositories.DiscountRepository>();

        // Register Checkout Service
        services.AddScoped<VisionCare.Application.Interfaces.Payment.ICheckoutService,
            VisionCare.Infrastructure.Services.Payment.CheckoutService>();

        // Register Payment Configuration
        services.AddScoped<VisionCare.Application.Interfaces.Payment.IPaymentConfiguration,
            VisionCare.Infrastructure.Services.Payment.PaymentConfiguration>();

        // Register Booking Service
        services.AddScoped<VisionCare.Application.Interfaces.Booking.IBookingService, 
            VisionCare.Application.Services.Booking.BookingService>();

        // EHR repositories
        services.AddScoped<IEncounterRepository, EncounterRepository>();
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // EHR services (Application)
        services.AddScoped<IEncounterService, EncounterService>();
        services.AddScoped<IPrescriptionService, PrescriptionService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
