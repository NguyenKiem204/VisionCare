using VisionCare.Application.DTOs.WorkShiftDto;

namespace VisionCare.Application.Interfaces.WorkShifts;

public interface IWorkShiftService
{
    Task<IEnumerable<WorkShiftDto>> GetAllWorkShiftsAsync();
    Task<WorkShiftDto?> GetWorkShiftByIdAsync(int id);
    Task<WorkShiftDto> CreateWorkShiftAsync(CreateWorkShiftRequest request);
    Task<WorkShiftDto> UpdateWorkShiftAsync(int id, UpdateWorkShiftRequest request);
    Task<bool> DeleteWorkShiftAsync(int id);
    Task<IEnumerable<WorkShiftDto>> GetActiveWorkShiftsAsync();
    Task<IEnumerable<WorkShiftDto>> SearchWorkShiftsAsync(WorkShiftSearchRequest request);
}

