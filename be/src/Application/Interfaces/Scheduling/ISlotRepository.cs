using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Scheduling;

public interface ISlotRepository
{
    Task<IEnumerable<Slot>> GetAllAsync();
    Task<Slot?> GetByIdAsync(int id);
    Task<IEnumerable<Slot>> GetByServiceTypeAsync(int serviceTypeId);
    Task<IEnumerable<Slot>> GetAvailableSlotsAsync(
        int doctorId,
        DateOnly scheduleDate,
        int? serviceTypeId
    );
    Task<Slot> AddAsync(Slot slot);
    Task UpdateAsync(Slot slot);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> TimeSlotExistsAsync(
        TimeOnly startTime,
        TimeOnly endTime,
        int serviceTypeId,
        int? excludeId = null
    );
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Slot>> GetPagedAsync(int page, int pageSize);
}
