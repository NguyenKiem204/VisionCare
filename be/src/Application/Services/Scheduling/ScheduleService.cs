using AutoMapper;
using VisionCare.Application.DTOs.ScheduleDto;
using VisionCare.Application.DTOs.SlotDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Scheduling;

public class ScheduleService : IScheduleService
{
    private readonly ISlotRepository _slotRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IMapper _mapper;

    public ScheduleService(
        ISlotRepository slotRepository,
        IScheduleRepository scheduleRepository,
        IMapper mapper
    )
    {
        _slotRepository = slotRepository;
        _scheduleRepository = scheduleRepository;
        _mapper = mapper;
    }

    // Slot management
    public async Task<IEnumerable<SlotDto>> GetAllSlotsAsync()
    {
        var slots = await _slotRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SlotDto>>(slots);
    }

    public async Task<SlotDto?> GetSlotByIdAsync(int id)
    {
        var slot = await _slotRepository.GetByIdAsync(id);
        return slot != null ? _mapper.Map<SlotDto>(slot) : null;
    }

    public async Task<IEnumerable<SlotDto>> GetSlotsByServiceTypeAsync(int serviceTypeId)
    {
        var slots = await _slotRepository.GetByServiceTypeAsync(serviceTypeId);
        return _mapper.Map<IEnumerable<SlotDto>>(slots);
    }

    public async Task<SlotDto> CreateSlotAsync(CreateSlotRequest request)
    {
        // Check if time slot already exists for this service type
        if (
            await _slotRepository.TimeSlotExistsAsync(
                request.StartTime,
                request.EndTime,
                request.ServiceTypeId
            )
        )
        {
            throw new ValidationException("Time slot already exists for this service type.");
        }

        // Use AutoMapper to create entity from DTO
        var slot = _mapper.Map<Slot>(request);
        slot.Created = DateTime.UtcNow;

        var createdSlot = await _slotRepository.AddAsync(slot);
        return _mapper.Map<SlotDto>(createdSlot);
    }

    public async Task<SlotDto> UpdateSlotAsync(int id, UpdateSlotRequest request)
    {
        var existingSlot = await _slotRepository.GetByIdAsync(id);
        if (existingSlot == null)
        {
            throw new NotFoundException($"Slot with ID {id} not found.");
        }

        // Check if new time slot conflicts with existing slots
        if (
            await _slotRepository.TimeSlotExistsAsync(
                request.StartTime,
                request.EndTime,
                existingSlot.ServiceTypeId,
                id
            )
        )
        {
            throw new ValidationException("Time slot already exists for this service type.");
        }

        // Use domain method to update time
        existingSlot.UpdateTime(request.StartTime, request.EndTime);

        await _slotRepository.UpdateAsync(existingSlot);
        return _mapper.Map<SlotDto>(existingSlot);
    }

    public async Task<bool> DeleteSlotAsync(int id)
    {
        var existingSlot = await _slotRepository.GetByIdAsync(id);
        if (existingSlot == null)
        {
            return false;
        }

        await _slotRepository.DeleteAsync(id);
        return true;
    }

    // Schedule management
    public async Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync()
    {
        var schedules = await _scheduleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }

    public async Task<ScheduleDto?> GetScheduleByIdAsync(int id)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(id);
        return schedule != null ? _mapper.Map<ScheduleDto>(schedule) : null;
    }

    public async Task<IEnumerable<ScheduleDto>> GetSchedulesByDoctorAsync(int doctorId)
    {
        var schedules = await _scheduleRepository.GetByDoctorIdAsync(doctorId);
        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }

    public async Task<IEnumerable<ScheduleDto>> GetSchedulesByDoctorAndDateAsync(
        int doctorId,
        DateOnly scheduleDate
    )
    {
        var schedules = await _scheduleRepository.GetByDoctorAndDateAsync(doctorId, scheduleDate);
        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }

    public async Task<ScheduleDto> CreateScheduleAsync(CreateScheduleRequest request)
    {
        // Check if schedule already exists for this doctor, slot, and date
        if (
            await _scheduleRepository.ScheduleExistsAsync(
                request.DoctorId,
                request.SlotId,
                request.ScheduleDate
            )
        )
        {
            throw new ValidationException(
                "Schedule already exists for this doctor, slot, and date."
            );
        }

        // Use AutoMapper to create entity from DTO
        var schedule = _mapper.Map<Schedule>(request);
        schedule.Created = DateTime.UtcNow;

        var createdSchedule = await _scheduleRepository.AddAsync(schedule);
        return _mapper.Map<ScheduleDto>(createdSchedule);
    }

    public async Task<ScheduleDto> UpdateScheduleAsync(int id, UpdateScheduleRequest request)
    {
        var existingSchedule = await _scheduleRepository.GetByIdAsync(id);
        if (existingSchedule == null)
        {
            throw new NotFoundException($"Schedule with ID {id} not found.");
        }

        // Use domain methods to update status
        if (!string.IsNullOrEmpty(request.Status))
        {
            switch (request.Status)
            {
                case "Available":
                    existingSchedule.MarkAsAvailable();
                    break;
                case "Booked":
                    existingSchedule.MarkAsBooked();
                    break;
                case "Blocked":
                    existingSchedule.Block();
                    break;
            }
        }

        await _scheduleRepository.UpdateAsync(existingSchedule);
        return _mapper.Map<ScheduleDto>(existingSchedule);
    }

    public async Task<bool> DeleteScheduleAsync(int id)
    {
        var existingSchedule = await _scheduleRepository.GetByIdAsync(id);
        if (existingSchedule == null)
        {
            return false;
        }

        await _scheduleRepository.DeleteAsync(id);
        return true;
    }

    // Availability checking
    public async Task<IEnumerable<ScheduleDto>> GetAvailableSlotsAsync(
        AvailableSlotsRequest request
    )
    {
        var schedules = await _scheduleRepository.GetAvailableSchedulesAsync(
            request.DoctorId,
            request.ScheduleDate,
            request.ServiceTypeId
        );

        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }

    public async Task<bool> IsSlotAvailableAsync(int doctorId, int slotId, DateOnly scheduleDate)
    {
        var schedule = await _scheduleRepository.GetByDoctorSlotAndDateAsync(
            doctorId,
            slotId,
            scheduleDate
        );
        return schedule?.IsValidForBooking() ?? false;
    }

    public async Task<ScheduleDto> BookSlotAsync(int doctorId, int slotId, DateOnly scheduleDate)
    {
        var schedule = await _scheduleRepository.GetByDoctorSlotAndDateAsync(
            doctorId,
            slotId,
            scheduleDate
        );
        if (schedule == null)
        {
            throw new NotFoundException(
                "Schedule not found for the specified doctor, slot, and date."
            );
        }

        if (!schedule.IsValidForBooking())
        {
            throw new ValidationException("Slot is not available for booking.");
        }

        // Use domain method to mark as booked
        schedule.MarkAsBooked();

        await _scheduleRepository.UpdateAsync(schedule);
        return _mapper.Map<ScheduleDto>(schedule);
    }

    public async Task<ScheduleDto> BlockSlotAsync(
        int doctorId,
        int slotId,
        DateOnly scheduleDate,
        string? reason = null
    )
    {
        var schedule = await _scheduleRepository.GetByDoctorSlotAndDateAsync(
            doctorId,
            slotId,
            scheduleDate
        );
        if (schedule == null)
        {
            throw new NotFoundException(
                "Schedule not found for the specified doctor, slot, and date."
            );
        }

        // Use domain method to block
        schedule.Block(reason);

        await _scheduleRepository.UpdateAsync(schedule);
        return _mapper.Map<ScheduleDto>(schedule);
    }

    public async Task<ScheduleDto> UnblockSlotAsync(int doctorId, int slotId, DateOnly scheduleDate)
    {
        var schedule = await _scheduleRepository.GetByDoctorSlotAndDateAsync(
            doctorId,
            slotId,
            scheduleDate
        );
        if (schedule == null)
        {
            throw new NotFoundException(
                "Schedule not found for the specified doctor, slot, and date."
            );
        }

        // Use domain method to mark as available
        schedule.MarkAsAvailable();

        await _scheduleRepository.UpdateAsync(schedule);
        return _mapper.Map<ScheduleDto>(schedule);
    }

    // Search and pagination
    public async Task<IEnumerable<ScheduleDto>> SearchSchedulesAsync(ScheduleSearchRequest request)
    {
        var schedules = await _scheduleRepository.GetByDoctorIdAsync(request.DoctorId ?? 0);

        // Apply additional filters
        if (request.FromDate.HasValue)
        {
            schedules = schedules.Where(s => s.ScheduleDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            schedules = schedules.Where(s => s.ScheduleDate <= request.ToDate.Value);
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            schedules = schedules.Where(s => s.Status == request.Status);
        }

        return _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
    }

    public async Task<int> GetTotalSchedulesCountAsync()
    {
        return await _scheduleRepository.GetTotalCountAsync();
    }
}
