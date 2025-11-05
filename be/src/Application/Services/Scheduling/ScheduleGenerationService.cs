using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.ServiceTypes;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Scheduling;

public class ScheduleGenerationService : IScheduleGenerationService
{
    private readonly IWeeklyScheduleRepository _weeklyScheduleRepository;
    private readonly IDoctorScheduleRepository _doctorScheduleRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IDoctorAbsenceRepository _absenceRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ISlotRepository _slotRepository;
    private readonly IServiceTypeRepository _serviceTypeRepository;
    private readonly ISlotGenerationService _slotGenerationService;

    public ScheduleGenerationService(
        IWeeklyScheduleRepository weeklyScheduleRepository,
        IDoctorScheduleRepository doctorScheduleRepository,
        IScheduleRepository scheduleRepository,
        IDoctorAbsenceRepository absenceRepository,
        IDoctorRepository doctorRepository,
        ISlotRepository slotRepository,
        IServiceTypeRepository serviceTypeRepository,
        ISlotGenerationService slotGenerationService
    )
    {
        _weeklyScheduleRepository = weeklyScheduleRepository;
        _doctorScheduleRepository = doctorScheduleRepository;
        _scheduleRepository = scheduleRepository;
        _absenceRepository = absenceRepository;
        _doctorRepository = doctorRepository;
        _slotRepository = slotRepository;
        _serviceTypeRepository = serviceTypeRepository;
        _slotGenerationService = slotGenerationService;
    }

    public async Task<int> GenerateSchedulesForDoctorAsync(int doctorId, int daysAhead = 14)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Today);
        var endDate = startDate.AddDays(daysAhead);
        await _scheduleRepository.DeleteByDoctorAndDateRangeAsync(doctorId, startDate, endDate);
        return await GenerateSchedulesForDateRangeAsync(doctorId, startDate, endDate);
    }

    public async Task<int> GenerateSchedulesForAllDoctorsAsync(int daysAhead = 14)
    {
        var doctors = await _doctorRepository.GetAllAsync();
        var doctorIds = doctors.Select(d => d.AccountId ?? 0).Where(id => id > 0).Distinct();

        int totalGenerated = 0;
        foreach (var doctorId in doctorIds)
        {
            totalGenerated += await GenerateSchedulesFromAllDoctorSchedulesAsync(
                doctorId,
                daysAhead
            );
        }

        return totalGenerated;
    }

    public async Task<int> GenerateSchedulesForDateRangeAsync(
        int doctorId,
        DateOnly startDate,
        DateOnly endDate
    )
    {
        var weeklySchedules = await _weeklyScheduleRepository.GetActiveByDoctorIdAsync(doctorId);
        if (!weeklySchedules.Any())
            return 0;

        var absences = await _absenceRepository.GetByDoctorAndDateRangeAsync(
            doctorId,
            startDate,
            endDate
        );

        int generatedCount = 0;
        var currentDate = startDate;

        while (currentDate <= endDate)
        {
            var dayOfWeek = currentDate.DayOfWeek;

            var schedulesForDay = weeklySchedules.Where(ws => ws.DayOfWeek == dayOfWeek);

            foreach (var weeklySchedule in schedulesForDay)
            {
                var isAbsent = absences.Any(a => a.ContainsDate(currentDate));
                if (isAbsent)
                    continue;

                var exists = await _scheduleRepository.ScheduleExistsAsync(
                    doctorId,
                    weeklySchedule.SlotId,
                    currentDate
                );

                if (!exists)
                {
                    var schedule = new Schedule
                    {
                        DoctorId = doctorId,
                        SlotId = weeklySchedule.SlotId,
                        ScheduleDate = currentDate,
                        Status = "Available",
                        Created = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                    };

                    await _scheduleRepository.AddAsync(schedule);
                    generatedCount++;
                }
            }

            currentDate = currentDate.AddDays(1);
        }

        return generatedCount;
    }

    public async Task<int> GenerateSchedulesFromDoctorScheduleAsync(
        int doctorScheduleId,
        DateOnly? startDate = null,
        DateOnly? endDate = null
    )
    {
        var doctorSchedule = await _doctorScheduleRepository.GetByIdAsync(doctorScheduleId);
        if (doctorSchedule == null || !doctorSchedule.IsActive)
        {
            return 0;
        }

        var effectiveStartDate = startDate ?? DateOnly.FromDateTime(DateTime.Today);
        var effectiveEndDate = endDate ?? doctorSchedule.EndDate ?? effectiveStartDate.AddDays(14);

        if (effectiveStartDate < doctorSchedule.StartDate)
        {
            effectiveStartDate = doctorSchedule.StartDate;
        }

        if (doctorSchedule.EndDate.HasValue && effectiveEndDate > doctorSchedule.EndDate.Value)
        {
            effectiveEndDate = doctorSchedule.EndDate.Value;
        }

        var absences = await _absenceRepository.GetByDoctorAndDateRangeAsync(
            doctorSchedule.DoctorId,
            effectiveStartDate,
            effectiveEndDate
        );

        var shift = doctorSchedule.Shift;
        if (shift == null)
        {
            return 0;
        }

        var serviceTypes = await _serviceTypeRepository.GetAllAsync();
        var defaultServiceType = serviceTypes.FirstOrDefault(st => st.DurationMinutes == 60);
        if (defaultServiceType == null)
        {
            var created = await _serviceTypeRepository.AddAsync(
                new Domain.Entities.ServiceType
                {
                    Name = "Default 60m",
                    DurationMinutes = 60,
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                }
            );
            defaultServiceType = created;
        }

        await _slotGenerationService.GenerateSlotsFromWorkShiftAsync(
            shift.Id,
            defaultServiceType.Id,
            false
        );
        var serviceTypeSlots = await _slotRepository.GetByServiceTypeAsync(defaultServiceType.Id);
        var slots = serviceTypeSlots
            .Where(s => s.StartTime >= shift.StartTime && s.EndTime <= shift.EndTime)
            .ToList();

        const int BUFFER_MINUTES = 15;
        var slotDurationMinutes = defaultServiceType.DurationMinutes;
        var step = slotDurationMinutes + BUFFER_MINUTES;
        var alignedStarts = new HashSet<string>();
        var cursor = shift.StartTime;
        while (cursor.AddMinutes(slotDurationMinutes) <= shift.EndTime)
        {
            alignedStarts.Add(cursor.ToString("HH:mm"));
            cursor = cursor.AddMinutes(step);
        }
        slots = slots.Where(s => alignedStarts.Contains(s.StartTime.ToString("HH:mm"))).ToList();

        if (!slots.Any())
        {
            return 0;
        }

        int generatedCount = 0;
        var currentDate = effectiveStartDate;

        while (currentDate <= effectiveEndDate)
        {
            if (!doctorSchedule.AppliesToDate(currentDate))
            {
                currentDate = currentDate.AddDays(1);
                continue;
            }

            var isAbsent = absences.Any(a => a.ContainsDate(currentDate));
            if (isAbsent)
            {
                currentDate = currentDate.AddDays(1);
                continue;
            }

            foreach (var slot in slots)
            {
                var exists = await _scheduleRepository.ScheduleExistsAsync(
                    doctorSchedule.DoctorId,
                    slot.Id,
                    currentDate
                );

                if (!exists)
                {
                    var isResourceAvailable = await _scheduleRepository.IsResourceAvailableAsync(
                        doctorSchedule.RoomId,
                        doctorSchedule.EquipmentId,
                        slot.Id,
                        currentDate
                    );

                    if (!isResourceAvailable)
                    {
                        continue;
                    }

                    var schedule = new Schedule
                    {
                        DoctorId = doctorSchedule.DoctorId,
                        SlotId = slot.Id,
                        ScheduleDate = currentDate,
                        Status = "Available",
                        RoomId = doctorSchedule.RoomId,
                        EquipmentId = doctorSchedule.EquipmentId,
                        Created = DateTime.UtcNow,
                        LastModified = DateTime.UtcNow,
                    };

                    await _scheduleRepository.AddAsync(schedule);
                    generatedCount++;
                }
            }

            currentDate = currentDate.AddDays(1);
        }

        return generatedCount;
    }

    public async Task<int> GenerateSchedulesFromAllDoctorSchedulesAsync(
        int doctorId,
        int daysAhead = 14
    )
    {
        var doctorSchedules = await _doctorScheduleRepository.GetActiveByDoctorIdAsync(doctorId);
        if (!doctorSchedules.Any())
        {
            return 0;
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        var defaultStart = today;
        var defaultEnd = defaultStart.AddDays(daysAhead);

        var minStart = doctorSchedules.Min(ds => ds.StartDate);
        var maxEnd = doctorSchedules.Any(ds => ds.EndDate.HasValue)
            ? doctorSchedules.Where(ds => ds.EndDate.HasValue).Max(ds => ds.EndDate!.Value)
            : (DateOnly?)null;

        var effectiveStart = minStart > defaultStart ? minStart : defaultStart;
        var effectiveEnd = maxEnd.HasValue && maxEnd.Value < defaultEnd ? maxEnd.Value : defaultEnd;

        int totalGenerated = 0;

        await _scheduleRepository.DeleteByDoctorAndDateRangeAsync(
            doctorId,
            effectiveStart,
            effectiveEnd
        );

        foreach (var doctorSchedule in doctorSchedules)
        {
            var count = await GenerateSchedulesFromDoctorScheduleAsync(
                doctorSchedule.Id,
                effectiveStart,
                effectiveEnd
            );
            totalGenerated += count;
        }

        return totalGenerated;
    }
}
