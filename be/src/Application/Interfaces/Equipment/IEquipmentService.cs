using VisionCare.Application.DTOs.EquipmentDto;

namespace VisionCare.Application.Interfaces.Equipment;

public interface IEquipmentService
{
    Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync();
    Task<EquipmentDto?> GetEquipmentByIdAsync(int id);
    Task<EquipmentDto> CreateEquipmentAsync(CreateEquipmentRequest request);
    Task<EquipmentDto> UpdateEquipmentAsync(int id, UpdateEquipmentRequest request);
    Task<bool> DeleteEquipmentAsync(int id);
    Task<(IEnumerable<EquipmentDto> items, int totalCount)> SearchEquipmentAsync(
        string? keyword,
        string? status,
        string? location,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    );
}
