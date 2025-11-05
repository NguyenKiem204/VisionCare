using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.ServiceTypes;
using VisionCare.Application.Interfaces.WorkShifts;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Scheduling;

public class SlotGenerationService : ISlotGenerationService
{
    private readonly ISlotRepository _slotRepository;
    private readonly IWorkShiftRepository _workShiftRepository;
    private readonly IServiceTypeRepository _serviceTypeRepository;

    public SlotGenerationService(
        ISlotRepository slotRepository,
        IWorkShiftRepository workShiftRepository,
        IServiceTypeRepository serviceTypeRepository
    )
    {
        _slotRepository = slotRepository;
        _workShiftRepository = workShiftRepository;
        _serviceTypeRepository = serviceTypeRepository;
    }

    public async Task<int> GenerateSlotsFromWorkShiftAsync(
        int shiftId,
        int serviceTypeId,
        bool overwriteExisting = false
    )
    {
        const int BUFFER_MINUTES = 15;

        var shift = await _workShiftRepository.GetByIdAsync(shiftId);
        if (shift == null)
        {
            throw new NotFoundException($"WorkShift with ID {shiftId} not found.");
        }

        if (!shift.IsActive)
        {
            throw new ValidationException($"WorkShift with ID {shiftId} is not active.");
        }

        var serviceType = await _serviceTypeRepository.GetByIdAsync(serviceTypeId);
        if (serviceType == null)
        {
            throw new NotFoundException($"ServiceType with ID {serviceTypeId} not found.");
        }

        var shiftDurationMinutes = shift.GetDurationMinutes();
        var slotDurationMinutes = serviceType.DurationMinutes;

        if (slotDurationMinutes <= 0)
        {
            throw new ValidationException(
                $"ServiceType {serviceTypeId} has invalid duration: {slotDurationMinutes} minutes."
            );
        }

        if (slotDurationMinutes > shiftDurationMinutes)
        {
            throw new ValidationException(
                $"ServiceType duration ({slotDurationMinutes} minutes) exceeds WorkShift duration ({shiftDurationMinutes} minutes)."
            );
        }

        var totalSlotTime = slotDurationMinutes + BUFFER_MINUTES;
        var numberOfSlots = shiftDurationMinutes / totalSlotTime;
        if (numberOfSlots == 0)
        {
            return 0;
        }

        int createdCount = 0;
        var currentTime = shift.StartTime;

        while (currentTime.AddMinutes(slotDurationMinutes) <= shift.EndTime)
        {
            var endTime = currentTime.AddMinutes(slotDurationMinutes);

            if (endTime > shift.EndTime)
            {
                break;
            }

            var exists = await _slotRepository.TimeSlotExistsAsync(
                currentTime,
                endTime,
                serviceTypeId
            );

            if (exists && !overwriteExisting)
            {
                currentTime = endTime.AddMinutes(BUFFER_MINUTES);
                continue;
            }

            var slot = new Slot
            {
                StartTime = currentTime,
                EndTime = endTime,
                ServiceTypeId = serviceTypeId,
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            };

            await _slotRepository.AddAsync(slot);
            createdCount++;

            currentTime = endTime.AddMinutes(BUFFER_MINUTES);
        }

        return createdCount;
    }

    public async Task<Dictionary<int, int>> GenerateSlotsFromWorkShiftForAllServiceTypesAsync(
        int shiftId,
        bool overwriteExisting = false
    )
    {
        var serviceTypes = await _serviceTypeRepository.GetAllAsync();

        var results = new Dictionary<int, int>();

        foreach (var serviceType in serviceTypes)
        {
            try
            {
                var count = await GenerateSlotsFromWorkShiftAsync(
                    shiftId,
                    serviceType.Id,
                    overwriteExisting
                );
                results[serviceType.Id] = count;
            }
            catch (Exception)
            {
                results[serviceType.Id] = -1;
            }
        }

        return results;
    }

    public async Task<Dictionary<string, int>> GenerateSlotsForAllWorkShiftsAsync(
        bool overwriteExisting = false
    )
    {
        var shifts = await _workShiftRepository.GetActiveShiftsAsync();

        var results = new Dictionary<string, int>();

        foreach (var shift in shifts)
        {
            var serviceTypeResults = await GenerateSlotsFromWorkShiftForAllServiceTypesAsync(
                shift.Id,
                overwriteExisting
            );
            var totalSlots = serviceTypeResults.Values.Where(v => v > 0).Sum();
            results[$"Shift_{shift.Id}_{shift.ShiftName}"] = totalSlots;
        }

        return results;
    }
}
