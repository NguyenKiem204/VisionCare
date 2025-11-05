using Microsoft.Extensions.Logging;
using VisionCare.Application.DTOs.BookingDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Booking;
using VisionCare.Application.Interfaces.Customers;
using VisionCare.Application.Interfaces.Payment;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Application.Interfaces.Users;
using VisionCare.Domain.Entities;
using VisionCare.Domain.Enums;
using VisionCare.Domain.Services;

namespace VisionCare.Application.Services.Booking;

public class BookingService : IBookingService
{
    private readonly IBookingHoldCacheService _holdCache;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly ICustomerService _customerService;
    private readonly IGuestAccountService _guestAccountService;
    private readonly IServiceDetailRepository _serviceDetailRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly ICheckoutService _checkoutService;
    private readonly IVNPayService _vnPayService;
    private readonly IPaymentConfiguration _paymentConfiguration;
    private readonly ILogger<BookingService> _logger;
    private const int DUPLICATE_CHECK_WINDOW_SECONDS = 30;

    public BookingService(
        IBookingHoldCacheService holdCache,
        IScheduleRepository scheduleRepository,
        IAppointmentRepository appointmentRepository,
        ICustomerRepository customerRepository,
        IUserRepository userRepository,
        IUserService userService,
        ICustomerService customerService,
        IGuestAccountService guestAccountService,
        IServiceDetailRepository serviceDetailRepository,
        IDiscountRepository discountRepository,
        ICheckoutService checkoutService,
        IVNPayService vnPayService,
        IPaymentConfiguration paymentConfiguration,
        ILogger<BookingService> logger
    )
    {
        _holdCache = holdCache;
        _scheduleRepository = scheduleRepository;
        _appointmentRepository = appointmentRepository;
        _customerRepository = customerRepository;
        _userRepository = userRepository;
        _userService = userService;
        _customerService = customerService;
        _guestAccountService = guestAccountService;
        _serviceDetailRepository = serviceDetailRepository;
        _discountRepository = discountRepository;
        _checkoutService = checkoutService;
        _vnPayService = vnPayService;
        _paymentConfiguration = paymentConfiguration;
        _logger = logger;
    }

    public async Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync(
        int doctorId,
        DateOnly date,
        int? serviceTypeId = null
    )
    {
        var schedules = await _scheduleRepository.GetAvailableSchedulesAsync(
            doctorId,
            date,
            serviceTypeId
        );

        var slots = new List<AvailableSlotDto>();

        foreach (var schedule in schedules)
        {
            if (schedule.Slot == null)
                continue;

            // Check if slot is on hold from Redis cache
            var holdData = await _holdCache.GetHoldBySlotAsync(doctorId, schedule.Slot.Id, date);
            var isOnHold = holdData != null;

            slots.Add(
                new AvailableSlotDto
                {
                    SlotId = schedule.Slot.Id,
                    StartTime = schedule.Slot.StartTime,
                    EndTime = schedule.Slot.EndTime,
                    DurationMinutes = schedule.Slot.GetDurationMinutes(),
                    AvailableCount = schedule.Status == "Available" ? 1 : 0,
                    IsAvailable = schedule.Status == "Available" && !isOnHold,
                    IsOnHold = isOnHold,
                    HoldByCustomerId = holdData?.CustomerId, // Trả về customerId đang hold
                }
            );
        }

        return slots;
    }

    public async Task<HoldSlotResponse> HoldSlotAsync(HoldSlotRequest request)
    {
        // Validate schedule exists and is available
        var schedule = await _scheduleRepository.GetByDoctorSlotAndDateAsync(
            request.DoctorId,
            request.SlotId,
            request.ScheduleDate
        );

        if (schedule == null)
            throw new NotFoundException("Schedule not found");

        if (schedule.Status != "Available")
            throw new ValidationException("Slot is not available");

        // Create hold in cache
        var holdToken = await _holdCache.CreateHoldAsync(request);
        var holdData = await _holdCache.GetHoldAsync(holdToken);

        if (holdData == null)
            throw new ValidationException("Failed to create hold");

        return new HoldSlotResponse { HoldToken = holdToken, ExpiresAt = holdData.ExpiresAt };
    }

    public async Task<bool> ValidateHoldAsync(string holdToken)
    {
        return await _holdCache.ValidateHoldAsync(holdToken);
    }

    public async Task ReleaseHoldAsync(string holdToken)
    {
        await _holdCache.ReleaseHoldAsync(holdToken);
    }

    public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request, string ipAddress)
    {
        // Validate hold token if provided
        if (!string.IsNullOrEmpty(request.HoldToken))
        {
            var isValid = await ValidateHoldAsync(request.HoldToken);
            if (!isValid)
                throw new ValidationException("Hold token is invalid or expired");
        }

        // Check for duplicate booking (within 30 seconds)
        var isDuplicate = await CheckDuplicateBookingAsync(request);
        if (isDuplicate)
            throw new ValidationException(
                "Duplicate booking request detected. Please wait before trying again."
            );

        // Validate schedule
        var schedule = await _scheduleRepository.GetByDoctorSlotAndDateAsync(
            request.DoctorId,
            request.SlotId,
            request.ScheduleDate
        );

        if (schedule == null || schedule.Status != "Available")
            throw new ValidationException("Slot is no longer available");

        // Get or create customer
        int patientId;
        if (request.CustomerId.HasValue)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId.Value);
            if (customer == null)
                throw new NotFoundException("Customer not found");
            patientId = customer.Id;
        }
        else
        {
            // Customer not logged in - find or create from phone/email
            if (string.IsNullOrEmpty(request.Phone) && string.IsNullOrEmpty(request.Email))
                throw new ValidationException("Customer ID, Phone, or Email is required");

            Customer? customer = null;

            // Try to find customer by phone
            if (!string.IsNullOrEmpty(request.Phone))
            {
                var customers = await _customerRepository.SearchCustomersAsync(
                    request.Phone,
                    null,
                    null,
                    null,
                    1,
                    10
                );
                customer = customers.items.FirstOrDefault();
            }

            // If not found by phone, try to find by email through account
            if (customer == null && !string.IsNullOrEmpty(request.Email))
            {
                var user = await _userService.GetUserByEmailAsync(request.Email);
                if (user != null)
                {
                    var customerDto = await _customerService.GetCustomerByAccountIdAsync(user.Id);
                    if (customerDto != null)
                    {
                        customer = await _customerRepository.GetByIdAsync(user.Id);
                    }
                }
            }

            // If still not found, create new customer with account using IGuestAccountService
            if (customer == null)
            {
                if (string.IsNullOrEmpty(request.Email))
                    throw new ValidationException(
                        "Email is required to create new customer account"
                    );

                customer = await _guestAccountService.FindOrCreateGuestCustomerAsync(
                    request.Email,
                    request.Phone,
                    request.CustomerName
                );
            }

            patientId = customer.Id;
        }

        // Get service detail for pricing
        var serviceDetail = await _serviceDetailRepository.GetByIdAsync(request.ServiceDetailId);
        if (serviceDetail == null)
            throw new NotFoundException("Service detail not found");

        // Calculate cost
        var baseCost = serviceDetail.Cost;
        decimal? discountPercent = null;

        if (request.DiscountId.HasValue)
        {
            var discount = await _discountRepository.GetByIdAsync(request.DiscountId.Value);
            if (discount != null)
                discountPercent = discount.DiscountPercent;
        }

        var actualCost = discountPercent.HasValue
            ? serviceDetail.CalculateDiscountedCost(discountPercent.Value)
            : baseCost;

        // Generate appointment code
        var appointmentCode = AppointmentCodeGenerator.Generate(request.ScheduleDate);

        // Create appointment datetime
        var appointmentDateTime = new DateTime(
            request.ScheduleDate.Year,
            request.ScheduleDate.Month,
            request.ScheduleDate.Day,
            request.StartTime.Hour,
            request.StartTime.Minute,
            request.StartTime.Second
        );

        // Create appointment using Domain entity
        var appointment = new Appointment
        {
            AppointmentDate = appointmentDateTime,
            AppointmentStatus = "Scheduled",
            DoctorId = request.DoctorId,
            PatientId = patientId,
            ServiceDetailId = request.ServiceDetailId,
            DiscountId = request.DiscountId,
            AppointmentCode = appointmentCode,
            PaymentStatus = PaymentStatus.Unpaid.ToString(),
            ActualCost = actualCost,
            Notes = request.Notes,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };

        var createdAppointment = await _appointmentRepository.AddAsync(appointment);

        // Update schedule status
        schedule.Status = "Booked";
        await _scheduleRepository.UpdateAsync(schedule);

        // Release hold if exists
        if (!string.IsNullOrEmpty(request.HoldToken))
        {
            await _holdCache.ReleaseHoldAsync(request.HoldToken);
        }

        _logger.LogInformation(
            "Booking created: AppointmentId={AppointmentId}, Code={Code}",
            createdAppointment.Id,
            appointmentCode
        );

        // Generate payment URL
        var paymentUrl = await InitiatePaymentAsync(createdAppointment.Id, "127.0.0.1");

        return new BookingResponse
        {
            AppointmentId = createdAppointment.Id,
            AppointmentCode = appointmentCode,
            PaymentStatus = PaymentStatus.Unpaid,
            TotalAmount = actualCost,
            PaymentUrl = paymentUrl,
        };
    }

    public async Task<bool> CheckDuplicateBookingAsync(CreateBookingRequest request)
    {
        // Check for duplicate bookings in the last 30 seconds
        var recentAppointments = await _appointmentRepository.GetByCustomerAsync(
            request.CustomerId ?? 0,
            DateTime.UtcNow.AddSeconds(-DUPLICATE_CHECK_WINDOW_SECONDS)
        );

        return recentAppointments.Any(a =>
            a.DoctorId == request.DoctorId
            && a.AppointmentDate?.Date == request.ScheduleDate.ToDateTime(TimeOnly.MinValue).Date
            && Math.Abs(
                (a.AppointmentDate?.TimeOfDay ?? TimeSpan.Zero).TotalMinutes
                    - request.StartTime.ToTimeSpan().TotalMinutes
            ) < 5
        );
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
            return null;

        string? doctorName = null;
        string? patientName = null;
        string? serviceName = null;

        if (appointment.DoctorId.HasValue)
        {
            doctorName = appointment.Doctor?.DoctorName;
        }

        if (appointment.PatientId.HasValue)
        {
            var patient = await _customerRepository.GetByIdAsync(appointment.PatientId.Value);
            patientName = patient?.CustomerName;
        }

        if (appointment.ServiceDetailId.HasValue)
        {
            var serviceDetail = await _serviceDetailRepository.GetByIdAsync(appointment.ServiceDetailId.Value);
            serviceName = serviceDetail?.Service?.Name;
        }

        return new BookingDto
        {
            Id = appointment.Id,
            AppointmentCode = appointment.AppointmentCode ?? string.Empty,
            AppointmentDate = appointment.AppointmentDate ?? DateTime.UtcNow,
            Status = appointment.AppointmentStatus ?? "Unknown",
            PaymentStatus = appointment.PaymentStatus ?? "Unknown",
            DoctorId = appointment.DoctorId ?? 0,
            DoctorName = doctorName,
            PatientId = appointment.PatientId ?? 0,
            PatientName = patientName,
            ServiceDetailId = appointment.ServiceDetailId ?? 0,
            ServiceName = serviceName,
            ActualCost = appointment.ActualCost,
            Notes = appointment.Notes,
            CreatedAt = appointment.Created,
        };
    }

    public async Task<BookingDto?> GetBookingByCodeAsync(string appointmentCode)
    {
        var appointments = await _appointmentRepository.SearchAppointmentsAsync(
            keyword: appointmentCode,
            status: null,
            doctorId: null,
            customerId: null,
            startDate: null,
            endDate: null,
            page: 1,
            pageSize: 10
        );

        var appointment = appointments.items.FirstOrDefault(a => a.AppointmentCode == appointmentCode);
        if (appointment == null)
            return null;

        string? doctorName = null;
        string? patientName = null;
        string? serviceName = null;

        if (appointment.DoctorId.HasValue)
        {
            doctorName = appointment.Doctor?.DoctorName;
        }

        if (appointment.PatientId.HasValue)
        {
            var patient = await _customerRepository.GetByIdAsync(appointment.PatientId.Value);
            patientName = patient?.CustomerName;
        }

        if (appointment.ServiceDetailId.HasValue)
        {
            var serviceDetail = await _serviceDetailRepository.GetByIdAsync(appointment.ServiceDetailId.Value);
            serviceName = serviceDetail?.Service?.Name;
        }

        return new BookingDto
        {
            Id = appointment.Id,
            AppointmentCode = appointment.AppointmentCode ?? string.Empty,
            AppointmentDate = appointment.AppointmentDate ?? DateTime.UtcNow,
            Status = appointment.AppointmentStatus ?? "Unknown",
            PaymentStatus = appointment.PaymentStatus ?? "Unknown",
            DoctorId = appointment.DoctorId ?? 0,
            DoctorName = doctorName,
            PatientId = appointment.PatientId ?? 0,
            PatientName = patientName,
            ServiceDetailId = appointment.ServiceDetailId ?? 0,
            ServiceName = serviceName,
            ActualCost = appointment.ActualCost,
            Notes = appointment.Notes,
            CreatedAt = appointment.Created,
        };
    }

    public async Task<BookingDto?> GetBookingByPhoneAsync(string phone)
    {
        var customers = await _customerRepository.SearchCustomersAsync(
            phone,
            null,
            null,
            null,
            1,
            10
        );
        var customer = customers.items.FirstOrDefault();
        if (customer == null)
            return null;

        var appointments = await _appointmentRepository.GetByCustomerAsync(customer.Id, null);
        var latestAppointment = appointments
            .OrderByDescending(a => a.AppointmentDate)
            .FirstOrDefault();
        if (latestAppointment == null)
            return null;

        return new BookingDto
        {
            Id = latestAppointment.Id,
            AppointmentCode = latestAppointment.AppointmentCode ?? string.Empty,
            AppointmentDate = latestAppointment.AppointmentDate ?? DateTime.UtcNow,
            Status = latestAppointment.AppointmentStatus ?? "Unknown",
            PaymentStatus = latestAppointment.PaymentStatus ?? "Unknown",
            DoctorId = latestAppointment.DoctorId ?? 0,
            PatientId = latestAppointment.PatientId ?? 0,
            ServiceDetailId = latestAppointment.ServiceDetailId ?? 0,
            ActualCost = latestAppointment.ActualCost,
            Notes = latestAppointment.Notes,
        };
    }

    public async Task<bool> CancelBookingAsync(int appointmentId, CancelBookingRequest request)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
            throw new NotFoundException("Appointment not found");

        if (appointment.AppointmentStatus == "Cancelled")
            throw new ValidationException("Appointment is already cancelled");

        appointment.Cancel(request.Reason);

        // Update payment status if needed
        if (request.RequestRefund && appointment.PaymentStatus == "Paid")
        {
            appointment.PaymentStatus = "Refunding";
        }

        await _appointmentRepository.UpdateAsync(appointment);

        return true;
    }

    public async Task<string> InitiatePaymentAsync(int appointmentId, string ipAddress)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
            throw new NotFoundException("Appointment not found");

        if (string.IsNullOrEmpty(appointment.AppointmentCode))
            throw new ValidationException("Appointment code is missing");

        decimal amount;
        if (appointment.ActualCost.HasValue)
        {
            amount = appointment.ActualCost.Value;
        }
        else
        {
            // Get service detail to get cost
            if (!appointment.ServiceDetailId.HasValue)
                throw new ValidationException("Service detail is missing");
            
            var serviceDetail = await _serviceDetailRepository.GetByIdAsync(appointment.ServiceDetailId.Value);
            if (serviceDetail == null)
                throw new NotFoundException("Service detail not found");
            
            amount = serviceDetail.Cost;
        }

        var orderInfo = $"Thanh toan dat lich kham - {appointment.AppointmentCode}";
        var returnUrl = _paymentConfiguration.GetReturnUrl();

        var paymentUrl = await _vnPayService.CreatePaymentUrlAsync(
            amount,
            orderInfo,
            appointment.AppointmentCode,
            returnUrl,
            ipAddress
        );

        return paymentUrl;
    }

    public async Task<bool> ProcessPaymentCallbackAsync(Dictionary<string, string> queryParams)
    {
        var callbackResult = await _vnPayService.VerifyCallbackAsync(queryParams);

        if (!callbackResult.IsSuccess)
        {
            _logger.LogWarning("Payment callback failed: {Message}", callbackResult.Message);
            return false;
        }

        // Find appointment by code
        var appointments = await _appointmentRepository.SearchAppointmentsAsync(
            keyword: callbackResult.OrderId,
            status: null,
            doctorId: null,
            customerId: null,
            startDate: null,
            endDate: null,
            page: 1,
            pageSize: 1
        );

        var appointment = appointments.items.FirstOrDefault(a => a.AppointmentCode == callbackResult.OrderId);
        
        if (appointment == null)
        {
            _logger.LogError("Appointment not found for code: {Code}", callbackResult.OrderId);
            return false;
        }

        // Update appointment payment status
        appointment.PaymentStatus = PaymentStatus.Paid.ToString();
        await _appointmentRepository.UpdateAsync(appointment);

        // Create checkout record
        await _checkoutService.CreateCheckoutAsync(new CreateCheckoutRequest
        {
            AppointmentId = appointment.Id,
            TransactionType = "VNPay",
            TransactionStatus = "Completed",
            TotalAmount = callbackResult.Amount,
            TransactionCode = $"VNP_{callbackResult.OrderId}",
            PaymentDate = DateTime.UtcNow,
            PaymentGateway = "VNPay",
            GatewayTransactionId = callbackResult.TransactionId,
            GatewayResponse = System.Text.Json.JsonSerializer.Serialize(queryParams),
        });

        _logger.LogInformation(
            "Payment processed successfully: AppointmentId={Id}, TransactionId={TxnId}",
            appointment.Id,
            callbackResult.TransactionId
        );

        return true;
    }
}
