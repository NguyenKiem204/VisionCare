using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Equipment;

public interface IEquipmentRepository
{
    Task<IEnumerable<Domain.Entities.Equipment>> GetAllAsync();
    Task<Domain.Entities.Equipment?> GetByIdAsync(int id);
    Task<Domain.Entities.Equipment> AddAsync(Domain.Entities.Equipment equipment);
    Task UpdateAsync(Domain.Entities.Equipment equipment);
    Task DeleteAsync(int id);
    Task<(IEnumerable<Domain.Entities.Equipment> items, int totalCount)> SearchAsync(
        string? keyword,
        string? status,
        string? location,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    );
}
