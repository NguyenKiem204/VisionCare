namespace VisionCare.Application.Interfaces.Scheduling;

public interface ISlotGenerationService
{
    Task<int> GenerateSlotsFromWorkShiftAsync(
        int shiftId,
        int serviceTypeId,
        bool overwriteExisting = false
    );

    Task<Dictionary<int, int>> GenerateSlotsFromWorkShiftForAllServiceTypesAsync(
        int shiftId,
        bool overwriteExisting = false
    );

    Task<Dictionary<string, int>> GenerateSlotsForAllWorkShiftsAsync(
        bool overwriteExisting = false
    );
}
