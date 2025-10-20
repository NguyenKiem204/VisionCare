using VisionCare.Application.DTOs.ScheduleDto;
using VisionCare.Application.DTOs.SlotDto;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface IScheduleService
{
    // Slot management
    Task<IEnumerable<SlotDto>> GetAllSlotsAsync();
    Task<SlotDto?> GetSlotByIdAsync(int id);
    Task<IEnumerable<SlotDto>> GetSlotsByServiceTypeAsync(int serviceTypeId);
    Task<SlotDto> CreateSlotAsync(CreateSlotRequest request);
    Task<SlotDto> UpdateSlotAsync(int id, UpdateSlotRequest request);
    Task<bool> DeleteSlotAsync(int id);

    // Schedule management
    Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync();
    Task<ScheduleDto?> GetScheduleByIdAsync(int id);
    Task<IEnumerable<ScheduleDto>> GetSchedulesByDoctorAsync(int doctorId);
    Task<IEnumerable<ScheduleDto>> GetSchedulesByDoctorAndDateAsync(
        int doctorId,
        DateOnly scheduleDate
    );
    Task<ScheduleDto> CreateScheduleAsync(CreateScheduleRequest request);
    Task<ScheduleDto> UpdateScheduleAsync(int id, UpdateScheduleRequest request);
    Task<bool> DeleteScheduleAsync(int id);

    // Availability checking
    Task<IEnumerable<ScheduleDto>> GetAvailableSlotsAsync(AvailableSlotsRequest request);
    Task<bool> IsSlotAvailableAsync(int doctorId, int slotId, DateOnly scheduleDate);
    Task<ScheduleDto> BookSlotAsync(int doctorId, int slotId, DateOnly scheduleDate);
    Task<ScheduleDto> BlockSlotAsync(
        int doctorId,
        int slotId,
        DateOnly scheduleDate,
        string? reason = null
    );
    Task<ScheduleDto> UnblockSlotAsync(int doctorId, int slotId, DateOnly scheduleDate);

    // Search and pagination
    Task<IEnumerable<ScheduleDto>> SearchSchedulesAsync(ScheduleSearchRequest request);
    Task<int> GetTotalSchedulesCountAsync();
}
