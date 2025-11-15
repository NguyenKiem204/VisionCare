using AutoMapper;
using VisionCare.Application.DTOs.AppointmentDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Appointments;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Appointments;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IServiceDetailRepository _serviceDetailRepository;
    private readonly IMapper _mapper;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IDoctorRepository doctorRepository,
        ICustomerRepository customerRepository,
        IScheduleRepository scheduleRepository,
        IServiceDetailRepository serviceDetailRepository,
        IMapper mapper
    )
    {
        _appointmentRepository = appointmentRepository;
        _doctorRepository = doctorRepository;
        _customerRepository = customerRepository;
        _scheduleRepository = scheduleRepository;
        _serviceDetailRepository = serviceDetailRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _appointmentRepository.GetAllAsync();
        return await EnrichAppointmentDtosAsync(appointments);
    }

    private async Task<AppointmentDto> EnrichAppointmentDtoAsync(Appointment appointment)
    {
        var dto = _mapper.Map<AppointmentDto>(appointment);
        
        // Load ServiceDetail and map ServiceName
        if (appointment.ServiceDetailId.HasValue)
        {
            var serviceDetail = await _serviceDetailRepository.GetByIdAsync(appointment.ServiceDetailId.Value);
            if (serviceDetail != null)
            {
                dto.ServiceName = serviceDetail.Service?.Name;
                dto.ServiceDetailName = serviceDetail.ServiceType?.Name;
            }
        }
        
        return dto;
    }

    private async Task<IEnumerable<AppointmentDto>> EnrichAppointmentDtosAsync(IEnumerable<Appointment> appointments)
    {
        var appointmentList = appointments.ToList();
        var dtos = _mapper.Map<List<AppointmentDto>>(appointmentList);
        
        // Batch load ServiceDetails
        var serviceDetailIds = appointmentList
            .Where(a => a.ServiceDetailId.HasValue)
            .Select(a => a.ServiceDetailId!.Value)
            .Distinct()
            .ToList();
        
        var serviceDetails = new Dictionary<int, ServiceDetail>();
        foreach (var serviceDetailId in serviceDetailIds)
        {
            var serviceDetail = await _serviceDetailRepository.GetByIdAsync(serviceDetailId);
            if (serviceDetail != null)
            {
                serviceDetails[serviceDetailId] = serviceDetail;
            }
        }
        
        // Map ServiceName to DTOs
        foreach (var dto in dtos)
        {
            var appointment = appointmentList.FirstOrDefault(a => a.Id == dto.Id);
            if (appointment?.ServiceDetailId.HasValue == true && 
                serviceDetails.TryGetValue(appointment.ServiceDetailId.Value, out var serviceDetail))
            {
                dto.ServiceName = serviceDetail.Service?.Name;
                dto.ServiceDetailName = serviceDetail.ServiceType?.Name;
            }
        }
        
        return dtos;
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null)
            return null;

        return await EnrichAppointmentDtoAsync(appointment);
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentRequest request)
    {
        // Validate doctor exists
        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
        if (doctor == null)
        {
            throw new NotFoundException($"Doctor with ID {request.DoctorId} not found.");
        }

        // Validate customer exists
        var customer = await _customerRepository.GetByIdAsync(request.PatientId);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with ID {request.PatientId} not found.");
        }

        // Check if doctor is available at the requested time
        if (!await IsDoctorAvailableAsync(request.DoctorId, request.AppointmentDate))
        {
            throw new ValidationException("Doctor is not available at the requested time.");
        }

        // Use AutoMapper to create entity from DTO
        var appointment = _mapper.Map<Appointment>(request);
        appointment.AppointmentStatus = "Pending";
        appointment.Created = DateTime.UtcNow;

        var createdAppointment = await _appointmentRepository.AddAsync(appointment);
        return await EnrichAppointmentDtoAsync(createdAppointment);
    }

    public async Task<AppointmentDto> UpdateAppointmentAsync(
        int id,
        UpdateAppointmentRequest request
    )
    {
        var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
        if (existingAppointment == null)
        {
            throw new NotFoundException($"Appointment with ID {id} not found.");
        }

        // If changing date/time, check availability
        if (
            request.AppointmentDate.HasValue
            && request.AppointmentDate != existingAppointment.AppointmentDate
        )
        {
            if (
                !await IsDoctorAvailableAsync(
                    existingAppointment.DoctorId ?? 0,
                    request.AppointmentDate.Value
                )
            )
            {
                throw new ValidationException("Doctor is not available at the requested time.");
            }
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingAppointment);

        await _appointmentRepository.UpdateAsync(existingAppointment);
        return await EnrichAppointmentDtoAsync(existingAppointment);
    }

    public async Task<bool> DeleteAppointmentAsync(int id)
    {
        var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
        if (existingAppointment == null)
        {
            return false;
        }

        await _appointmentRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(
        int doctorId,
        DateTime? date = null
    )
    {
        var appointments = await _appointmentRepository.GetByDoctorAsync(doctorId, date);
        return await EnrichAppointmentDtosAsync(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByCustomerAsync(
        int customerId,
        DateTime? date = null
    )
    {
        var appointments = await _appointmentRepository.GetByCustomerAsync(customerId, date);
        return await EnrichAppointmentDtosAsync(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateAsync(DateTime date)
    {
        var appointments = await _appointmentRepository.GetByDateAsync(date);
        return await EnrichAppointmentDtosAsync(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateRangeAsync(
        DateTime startDate,
        DateTime endDate
    )
    {
        var appointments = await _appointmentRepository.GetByDateRangeAsync(startDate, endDate);
        return await EnrichAppointmentDtosAsync(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(
        int? doctorId = null,
        int? customerId = null
    )
    {
        var appointments = await _appointmentRepository.GetUpcomingAsync(doctorId, customerId);
        return await EnrichAppointmentDtosAsync(appointments);
    }

    public async Task<AppointmentDto> ConfirmAppointmentAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        // Use domain method to confirm appointment
        appointment.Confirm();

        await _appointmentRepository.UpdateAsync(appointment);
        return await EnrichAppointmentDtoAsync(appointment);
    }

    public async Task<AppointmentDto> CancelAppointmentAsync(
        int appointmentId,
        string? reason = null
    )
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        // Use domain method to cancel appointment
        appointment.Cancel(reason);

        await _appointmentRepository.UpdateAsync(appointment);
        return await EnrichAppointmentDtoAsync(appointment);
    }

    public async Task<AppointmentDto> CompleteAppointmentAsync(
        int appointmentId,
        string? notes = null
    )
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        // Use domain method to complete appointment
        appointment.Complete(notes);

        await _appointmentRepository.UpdateAsync(appointment);
        return await EnrichAppointmentDtoAsync(appointment);
    }

    public async Task<AppointmentDto> RescheduleAppointmentAsync(
        int appointmentId,
        DateTime newDateTime
    )
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        if (!await IsDoctorAvailableAsync(appointment.DoctorId ?? 0, newDateTime))
        {
            throw new ValidationException("Doctor is not available at the requested time.");
        }

        // Use domain method to reschedule appointment
        appointment.Reschedule(newDateTime);

        await _appointmentRepository.UpdateAsync(appointment);
        return await EnrichAppointmentDtoAsync(appointment);
    }

    public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime dateTime)
    {
        return await _appointmentRepository.IsDoctorAvailableAsync(doctorId, dateTime);
    }

    public async Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date)
    {
        return await _appointmentRepository.GetAvailableTimeSlotsAsync(doctorId, date);
    }

    public async Task<bool> CheckAppointmentConflictAsync(
        int doctorId,
        DateTime dateTime,
        int? excludeAppointmentId = null
    )
    {
        return await _appointmentRepository.CheckConflictAsync(
            doctorId,
            dateTime,
            excludeAppointmentId
        );
    }

    public async Task<int> GetTotalAppointmentsCountAsync()
    {
        return await _appointmentRepository.GetTotalCountAsync();
    }

    public async Task<Dictionary<string, int>> GetAppointmentsByStatusStatsAsync()
    {
        return await _appointmentRepository.GetStatusStatsAsync();
    }

    public async Task<Dictionary<string, int>> GetAppointmentsByDoctorStatsAsync()
    {
        return await _appointmentRepository.GetDoctorStatsAsync();
    }

    public async Task<IEnumerable<AppointmentDto>> GetOverdueAppointmentsAsync()
    {
        var appointments = await _appointmentRepository.GetOverdueAsync();
        return await EnrichAppointmentDtosAsync(appointments);
    }

    public async Task<(IEnumerable<AppointmentDto> items, int totalCount)> SearchAppointmentsAsync(
        string? keyword,
        string? status,
        int? doctorId,
        int? customerId,
        DateTime? startDate,
        DateTime? endDate,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var result = await _appointmentRepository.SearchAppointmentsAsync(
            keyword,
            status,
            doctorId,
            customerId,
            startDate,
            endDate,
            page,
            pageSize,
            sortBy,
            desc
        );
        var appointmentDtos = await EnrichAppointmentDtosAsync(result.items);
        return (appointmentDtos, result.totalCount);
    }

    // Reschedule Workflow Methods
    public async Task<AppointmentDto> RequestRescheduleAsync(
        int appointmentId,
        DateTime proposedDateTime,
        string requestedBy,
        string? reason = null
    )
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        // Convert UTC to local time (Vietnam UTC+7) if needed
        DateTime localProposedDateTime = proposedDateTime;
        if (proposedDateTime.Kind == DateTimeKind.Utc)
        {
            // Convert UTC to Vietnam timezone (UTC+7)
            localProposedDateTime = proposedDateTime.AddHours(7);
            localProposedDateTime = DateTime.SpecifyKind(localProposedDateTime, DateTimeKind.Unspecified);
        }
        else if (proposedDateTime.Kind == DateTimeKind.Local)
        {
            // Convert to Unspecified to avoid timezone issues
            localProposedDateTime = DateTime.SpecifyKind(proposedDateTime, DateTimeKind.Unspecified);
        }

        // Validate: Only can request if paid
        if (appointment.PaymentStatus != "Paid")
        {
            throw new ValidationException("Chỉ có thể đề xuất đổi lịch sau khi thanh toán");
        }

        // Validate: Only can request if status is Confirmed, Scheduled, or Rescheduled
        // Rescheduled cho phép đề xuất đổi lịch lại
        if (appointment.AppointmentStatus != "Confirmed" && 
            appointment.AppointmentStatus != "Scheduled" && 
            appointment.AppointmentStatus != "Rescheduled")
        {
            throw new ValidationException("Chỉ có thể đề xuất đổi lịch khi trạng thái là Confirmed, Scheduled hoặc Rescheduled");
        }

        // Validate: Cannot request if already completed or cancelled
        if (appointment.AppointmentStatus == "Completed" || appointment.AppointmentStatus == "Cancelled")
        {
            throw new ValidationException("Không thể đề xuất đổi lịch cho appointment đã hoàn thành hoặc đã hủy");
        }

        // Validate: Cannot request if already has pending reschedule
        if (appointment.AppointmentStatus == "PendingReschedule")
        {
            throw new ValidationException("Đã có đề xuất đổi lịch đang chờ phê duyệt");
        }

        // Validate: Must be at least 24 hours before appointment
        if (appointment.AppointmentDate.HasValue)
        {
            var timeUntilAppointment = appointment.AppointmentDate.Value - DateTime.UtcNow;
            if (timeUntilAppointment.TotalHours < 24)
            {
                throw new ValidationException("Không thể đề xuất đổi lịch trong vòng 24 giờ trước giờ hẹn");
            }
        }

        // Validate: Proposed time must be in the future (at least 24 hours)
        // Compare with local time
        var localNow = DateTime.UtcNow.AddHours(7);
        var timeUntilProposed = localProposedDateTime - localNow;
        if (timeUntilProposed.TotalHours < 24)
        {
            throw new ValidationException("Thời gian đề xuất phải cách ít nhất 24 giờ");
        }

        // Validate: Proposed time cannot be more than 3 months in advance
        if (timeUntilProposed.TotalDays > 90)
        {
            throw new ValidationException("Thời gian đề xuất không được quá 3 tháng");
        }

        // Validate: Doctor must be available at proposed time
        // Nếu là bác sĩ tự đề xuất, chỉ check conflict với appointment khác, không check schedule
        // Nếu là customer đề xuất, check cả schedule và conflict
        if (requestedBy == "Doctor")
        {
            // Bác sĩ tự đề xuất: chỉ check conflict với appointment khác (trừ appointment hiện tại)
            var hasConflict = await CheckAppointmentConflictAsync(
                appointment.DoctorId ?? 0, 
                localProposedDateTime, 
                appointment.Id
            );
            if (hasConflict)
            {
                throw new ValidationException("Thời gian đề xuất đã có appointment khác");
            }
        }
        else
        {
            // Customer đề xuất: check cả schedule availability
            if (!await IsDoctorAvailableAsync(appointment.DoctorId ?? 0, localProposedDateTime))
            {
                throw new ValidationException("Bác sĩ không available ở thời gian đề xuất");
            }
        }

        // Use domain method to request reschedule (will convert to local time in Notes)
        appointment.RequestReschedule(localProposedDateTime, requestedBy, reason);

        await _appointmentRepository.UpdateAsync(appointment);
        return await EnrichAppointmentDtoAsync(appointment);
    }

    public async Task<AppointmentDto> ApproveRescheduleAsync(int appointmentId, string approvedBy)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        if (appointment.AppointmentStatus != "PendingReschedule")
        {
            throw new ValidationException("Không có đề xuất đổi lịch nào đang chờ phê duyệt");
        }

        // Validate: Người approve phải khác người request (trừ Staff có thể approve cho cả 2 bên)
        var requestedBy = ExtractRequestedBy(appointment.Notes);
        if (requestedBy == approvedBy && approvedBy != "Staff")
        {
            throw new ValidationException("Bạn không thể tự phê duyệt đề xuất đổi lịch của chính mình");
        }

        // Extract proposedDateTime from Notes
        var proposedDateTime = ExtractProposedDateTime(appointment.Notes);
        if (!proposedDateTime.HasValue)
        {
            // Log Notes for debugging
            var notesPreview = string.IsNullOrEmpty(appointment.Notes) 
                ? "null" 
                : appointment.Notes.Length > 100 
                    ? appointment.Notes.Substring(0, 100) + "..." 
                    : appointment.Notes;
            throw new ValidationException($"Không tìm thấy thời gian đề xuất trong appointment. Notes: {notesPreview}");
        }

        // Ensure DateTimeKind is Unspecified for database operations
        var proposedDateTimeValue = proposedDateTime.Value;
        if (proposedDateTimeValue.Kind != DateTimeKind.Unspecified)
        {
            proposedDateTimeValue = DateTime.SpecifyKind(proposedDateTimeValue, DateTimeKind.Unspecified);
        }

        // Validate availability based on who requested
        if (requestedBy == "Doctor")
        {
            // Nếu là bác sĩ đề xuất: chỉ check conflict với appointment khác (không check schedule)
            var hasConflict = await CheckAppointmentConflictAsync(
                appointment.DoctorId ?? 0, 
                proposedDateTimeValue, 
                appointment.Id
            );
            if (hasConflict)
            {
                throw new ValidationException("Thời gian đề xuất đã có appointment khác");
            }
            
            // Block old schedule slot (không giải phóng vì bác sĩ đã chuyển lịch, không làm slot cũ nữa)
            await BlockOldScheduleSlotAsync(appointment, "Bác sĩ đã đổi lịch sang thời gian khác");
            
            // Không cần book schedule slot mới vì bác sĩ có thể chọn bất kỳ thời gian nào
            // Chỉ book nếu có slot phù hợp (optional)
            try
            {
                await BookNewScheduleSlotAsync(appointment, proposedDateTimeValue);
            }
            catch (ValidationException)
            {
                // Nếu không tìm thấy slot phù hợp, bỏ qua (bác sĩ có thể chọn thời gian ngoài schedule)
                // Không throw error
            }
        }
        else
        {
            // Nếu là customer đề xuất: check cả schedule availability và conflict
            if (!await IsDoctorAvailableAsync(appointment.DoctorId ?? 0, proposedDateTimeValue))
            {
                throw new ValidationException("Bác sĩ không còn available ở thời gian đề xuất");
            }

            // Release old schedule slot (customer chỉ đổi lịch trong cùng schedule, nên giải phóng để người khác book)
            await ReleaseOldScheduleSlotAsync(appointment);

            // Book new schedule slot (bắt buộc cho customer)
            await BookNewScheduleSlotAsync(appointment, proposedDateTimeValue);
        }

        // Update appointment
        appointment.ApproveReschedule(proposedDateTimeValue);

        await _appointmentRepository.UpdateAsync(appointment);
        return await EnrichAppointmentDtoAsync(appointment);
    }

    public async Task<AppointmentDto> RejectRescheduleAsync(int appointmentId, string rejectedBy, string? reason = null)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null)
        {
            throw new NotFoundException($"Appointment with ID {appointmentId} not found.");
        }

        if (appointment.AppointmentStatus != "PendingReschedule")
        {
            throw new ValidationException("Không có đề xuất đổi lịch nào đang chờ phê duyệt");
        }

        // Validate: Người reject phải khác người request (trừ Staff có thể reject cho cả 2 bên)
        var requestedBy = ExtractRequestedBy(appointment.Notes);
        if (requestedBy == rejectedBy && rejectedBy != "Staff")
        {
            throw new ValidationException("Bạn không thể tự từ chối đề xuất đổi lịch của chính mình");
        }

        // Use domain method to reject
        appointment.RejectReschedule(reason);

        await _appointmentRepository.UpdateAsync(appointment);
        return await EnrichAppointmentDtoAsync(appointment);
    }

    public async Task<AppointmentDto> CounterRescheduleAsync(
        int appointmentId,
        DateTime proposedDateTime,
        string? reason = null
    )
    {
        // Counter-propose is essentially a new request, so reuse RequestRescheduleAsync
        return await RequestRescheduleAsync(appointmentId, proposedDateTime, "Counter", reason);
    }

    // Helper Methods
    private async Task ReleaseOldScheduleSlotAsync(Appointment appointment)
    {
        if (!appointment.DoctorId.HasValue || !appointment.AppointmentDate.HasValue)
            return;

        var oldDate = DateOnly.FromDateTime(appointment.AppointmentDate.Value);
        var oldTime = appointment.AppointmentDate.Value.TimeOfDay;

        var oldSchedules = await _scheduleRepository.GetByDoctorAndDateAsync(
            appointment.DoctorId.Value,
            oldDate
        );

        var oldSchedule = oldSchedules.FirstOrDefault(s =>
        {
            if (s.Slot == null) return false;
            var slotStart = s.Slot.StartTime.ToTimeSpan();
            return Math.Abs((oldTime - slotStart).TotalMinutes) < 5;
        });

        if (oldSchedule != null && oldSchedule.IsBooked())
        {
            oldSchedule.MarkAsAvailable();
            await _scheduleRepository.UpdateAsync(oldSchedule);
        }
    }

    private async Task BlockOldScheduleSlotAsync(Appointment appointment, string? reason = null)
    {
        if (!appointment.DoctorId.HasValue || !appointment.AppointmentDate.HasValue)
            return;

        var oldDate = DateOnly.FromDateTime(appointment.AppointmentDate.Value);
        var oldTime = appointment.AppointmentDate.Value.TimeOfDay;

        var oldSchedules = await _scheduleRepository.GetByDoctorAndDateAsync(
            appointment.DoctorId.Value,
            oldDate
        );

        var oldSchedule = oldSchedules.FirstOrDefault(s =>
        {
            if (s.Slot == null) return false;
            var slotStart = s.Slot.StartTime.ToTimeSpan();
            return Math.Abs((oldTime - slotStart).TotalMinutes) < 5;
        });

        if (oldSchedule != null)
        {
            // Block slot cũ vì bác sĩ đã chuyển lịch, không làm slot này nữa
            oldSchedule.Block(reason);
            await _scheduleRepository.UpdateAsync(oldSchedule);
        }
    }

    private async Task BookNewScheduleSlotAsync(Appointment appointment, DateTime newDateTime)
    {
        if (!appointment.DoctorId.HasValue)
            throw new ValidationException("Appointment must have a doctor");

        var newDate = DateOnly.FromDateTime(newDateTime);
        var newTime = newDateTime.TimeOfDay;
        var newTimeOnly = TimeOnly.FromTimeSpan(newTime);

        var newSchedules = await _scheduleRepository.GetByDoctorAndDateAsync(
            appointment.DoctorId.Value,
            newDate
        );

        // First, try to find a slot where the proposed time falls within [StartTime, EndTime)
        var newSchedule = newSchedules.FirstOrDefault(s =>
        {
            if (s.Slot == null) return false;
            var slotStart = s.Slot.StartTime;
            var slotEnd = s.Slot.EndTime;
            // Check if proposed time is within slot time range
            return newTimeOnly >= slotStart && newTimeOnly < slotEnd;
        });

        // If not found, try to find the closest slot (within 15 minutes of start time)
        if (newSchedule == null)
        {
            newSchedule = newSchedules
                .Where(s => s.Slot != null)
                .OrderBy(s =>
                {
                    var slotStart = s.Slot!.StartTime.ToTimeSpan();
                    return Math.Abs((newTime - slotStart).TotalMinutes);
                })
                .FirstOrDefault(s =>
                {
                    var slotStart = s.Slot!.StartTime.ToTimeSpan();
                    return Math.Abs((newTime - slotStart).TotalMinutes) <= 15;
                });
        }

        if (newSchedule == null)
        {
            // Log available slots for debugging
            var availableSlots = string.Join(", ", newSchedules
                .Where(s => s.Slot != null && s.IsAvailable())
                .Select(s => $"{s.Slot!.StartTime:HH:mm}-{s.Slot.EndTime:HH:mm}"));
            
            throw new ValidationException(
                $"Không tìm thấy schedule slot phù hợp cho thời gian mới ({newTimeOnly:HH:mm}). " +
                $"Các slot available: {(string.IsNullOrEmpty(availableSlots) ? "Không có" : availableSlots)}"
            );
        }

        if (!newSchedule.IsAvailable())
        {
            throw new ValidationException("Slot mới không còn trống");
        }

        newSchedule.MarkAsBooked();
        await _scheduleRepository.UpdateAsync(newSchedule);
    }

    private string? ExtractRequestedBy(string? notes)
    {
        if (string.IsNullOrEmpty(notes)) return null;
        
        // Find the last reschedule request to get the requester
        // Look for the last occurrence of [Customer], [Doctor], or [Counter] before "Đề xuất đổi lịch:"
        var prefix = "Đề xuất đổi lịch: ";
        var lastIndex = notes.LastIndexOf(prefix);
        if (lastIndex == -1)
            return null;
        
        // Find the [Role] tag before the last "Đề xuất đổi lịch:"
        var beforePrefix = notes.Substring(0, lastIndex);
        var customerIndex = beforePrefix.LastIndexOf("[Customer]");
        var doctorIndex = beforePrefix.LastIndexOf("[Doctor]");
        var counterIndex = beforePrefix.LastIndexOf("[Counter]");
        
        // Get the last occurrence
        var maxIndex = Math.Max(Math.Max(customerIndex, doctorIndex), counterIndex);
        if (maxIndex == -1)
            return null;
        
        if (maxIndex == customerIndex)
            return "Customer";
        if (maxIndex == doctorIndex)
            return "Doctor";
        if (maxIndex == counterIndex)
            return "Counter";
        
        return null;
    }

    private DateTime? ExtractProposedDateTime(string? notes)
    {
        if (string.IsNullOrEmpty(notes))
            return null;

        // Find the LAST occurrence of "Đề xuất đổi lịch: " (most recent request)
        var prefix = "Đề xuất đổi lịch: ";
        var lastIndex = notes.LastIndexOf(prefix);
        if (lastIndex == -1)
            return null;

        var startIndex = lastIndex + prefix.Length;
        // Find end of datetime - could be followed by ". Lý do:" or newline or end of string
        var endIndex = notes.IndexOf(".", startIndex);
        if (endIndex == -1)
        {
            // Try to find newline
            endIndex = notes.IndexOf("\n", startIndex);
            if (endIndex == -1)
                endIndex = notes.Length;
        }

        var dateTimeStr = notes.Substring(startIndex, endIndex - startIndex).Trim();
        
        // Try parsing with exact format first
        if (DateTime.TryParseExact(dateTimeStr, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out var result))
        {
            // Ensure DateTimeKind is Unspecified for PostgreSQL compatibility
            if (result.Kind != DateTimeKind.Unspecified)
            {
                result = DateTime.SpecifyKind(result, DateTimeKind.Unspecified);
            }
            return result;
        }

        // If exact parse fails, try to extract just the date part and time part separately
        // Pattern: dd/MM/yyyy HH:mm
        var parts = dateTimeStr.Split(' ');
        if (parts.Length >= 2)
        {
            var datePart = parts[0]; // dd/MM/yyyy
            var timePart = parts[1]; // HH:mm
            
            if (DateTime.TryParseExact($"{datePart} {timePart}", "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out result))
            {
                if (result.Kind != DateTimeKind.Unspecified)
                {
                    result = DateTime.SpecifyKind(result, DateTimeKind.Unspecified);
                }
                return result;
            }
        }

        return null;
    }
}
