using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.WorkShifts;

public interface IWorkShiftRepository
{
    Task<IEnumerable<WorkShift>> GetAllAsync();
    Task<WorkShift?> GetByIdAsync(int id);
    Task<WorkShift> AddAsync(WorkShift workShift);
    Task UpdateAsync(WorkShift workShift);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<WorkShift>> GetActiveShiftsAsync();
    Task<IEnumerable<WorkShift>> SearchAsync(
        string? keyword,
        bool? isActive,
        int page = 1,
        int pageSize = 10
    );
}

