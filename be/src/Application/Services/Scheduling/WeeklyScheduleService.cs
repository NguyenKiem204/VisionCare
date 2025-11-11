using AutoMapper;
using VisionCare.Application.DTOs;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Scheduling;

public class WeeklyScheduleService : IWeeklyScheduleService
{
    private readonly IWeeklyScheduleRepository _weeklyScheduleRepository;
    private readonly ISlotRepository _slotRepository;
    private readonly IMapper _mapper;

    public WeeklyScheduleService(
        IWeeklyScheduleRepository weeklyScheduleRepository,
        ISlotRepository slotRepository,
        IMapper mapper
    )
    {
        _weeklyScheduleRepository = weeklyScheduleRepository;
        _slotRepository = slotRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WeeklyScheduleDto>> GetByDoctorIdAsync(int doctorId)
    {
        var schedules = await _weeklyScheduleRepository.GetByDoctorIdAsync(doctorId);
        return await MapToDtosAsync(schedules);
    }

    public async Task<WeeklyScheduleDto?> GetByIdAsync(int id)
    {
        var schedule = await _weeklyScheduleRepository.GetByIdAsync(id);
        if (schedule == null)
            return null;

        return await MapToDtoAsync(schedule);
    }

    public async Task<WeeklyScheduleDto> CreateAsync(CreateWeeklyScheduleRequest request)
    {
        // Check if weekly schedule already exists
        var existing = await _weeklyScheduleRepository.GetByDoctorDayAndSlotAsync(
            request.DoctorId,
            request.DayOfWeek,
            request.SlotId
        );

        if (existing != null)
        {
            throw new ValidationException(
                "Weekly schedule already exists for this doctor, day, and slot."
            );
        }

        // Validate slot exists
        var slot = await _slotRepository.GetByIdAsync(request.SlotId);
        if (slot == null)
        {
            throw new NotFoundException($"Slot with ID {request.SlotId} not found.");
        }

        var weeklySchedule = new WeeklySchedule
        {
            DoctorId = request.DoctorId,
            DayOfWeek = request.DayOfWeek,
            SlotId = request.SlotId,
            IsActive = request.IsActive,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };

        var created = await _weeklyScheduleRepository.AddAsync(weeklySchedule);
        return await MapToDtoAsync(created);
    }

    public async Task<WeeklyScheduleDto> UpdateAsync(
        int id,
        UpdateWeeklyScheduleRequest request
    )
    {
        var existing = await _weeklyScheduleRepository.GetByIdAsync(id);
        if (existing == null)
        {
            throw new NotFoundException($"Weekly schedule with ID {id} not found.");
        }

        if (request.DayOfWeek.HasValue)
            existing.DayOfWeek = request.DayOfWeek.Value;

        if (request.SlotId.HasValue)
        {
            // Validate slot exists
            var slot = await _slotRepository.GetByIdAsync(request.SlotId.Value);
            if (slot == null)
            {
                throw new NotFoundException($"Slot with ID {request.SlotId.Value} not found.");
            }
            existing.SlotId = request.SlotId.Value;
        }

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                existing.Activate();
            else
                existing.Deactivate();
        }

        await _weeklyScheduleRepository.UpdateAsync(existing);
        return await MapToDtoAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _weeklyScheduleRepository.GetByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        await _weeklyScheduleRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<WeeklyScheduleDto>> GetActiveByDoctorIdAsync(int doctorId)
    {
        var schedules = await _weeklyScheduleRepository.GetActiveByDoctorIdAsync(doctorId);
        return await MapToDtosAsync(schedules);
    }

    private async Task<WeeklyScheduleDto> MapToDtoAsync(WeeklySchedule schedule)
    {
        var slot = await _slotRepository.GetByIdAsync(schedule.SlotId);
        var dayNames = new[]
        {
            "Chủ nhật",
            "Thứ hai",
            "Thứ ba",
            "Thứ tư",
            "Thứ năm",
            "Thứ sáu",
            "Thứ bảy"
        };

        return new WeeklyScheduleDto
        {
            Id = schedule.Id,
            DoctorId = schedule.DoctorId,
            DayOfWeek = schedule.DayOfWeek,
            DayOfWeekName = dayNames[(int)schedule.DayOfWeek],
            SlotId = schedule.SlotId,
            StartTime = slot?.StartTime ?? TimeOnly.MinValue,
            EndTime = slot?.EndTime ?? TimeOnly.MinValue,
            IsActive = schedule.IsActive,
            Created = schedule.Created,
            LastModified = schedule.LastModified,
        };
    }

    private async Task<IEnumerable<WeeklyScheduleDto>> MapToDtosAsync(
        IEnumerable<WeeklySchedule> schedules
    )
    {
        var dtos = new List<WeeklyScheduleDto>();
        foreach (var schedule in schedules)
        {
            dtos.Add(await MapToDtoAsync(schedule));
        }
        return dtos;
    }
}

