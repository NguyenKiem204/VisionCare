using AutoMapper;
using VisionCare.Application.DTOs.EquipmentDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.Equipment;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Equipment;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public EquipmentService(IEquipmentRepository equipmentRepository, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync()
    {
        var equipment = await _equipmentRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<EquipmentDto>>(equipment);
    }

    public async Task<EquipmentDto?> GetEquipmentByIdAsync(int id)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(id);
        return equipment != null ? _mapper.Map<EquipmentDto>(equipment) : null;
    }

    public async Task<EquipmentDto> CreateEquipmentAsync(CreateEquipmentRequest request)
    {
        var equipment = _mapper.Map<Domain.Entities.Equipment>(request);
        var createdEquipment = await _equipmentRepository.AddAsync(equipment);
        return _mapper.Map<EquipmentDto>(createdEquipment);
    }

    public async Task<EquipmentDto> UpdateEquipmentAsync(int id, UpdateEquipmentRequest request)
    {
        var existingEquipment = await _equipmentRepository.GetByIdAsync(id);
        if (existingEquipment == null)
        {
            throw new NotFoundException($"Equipment with ID {id} not found.");
        }

        _mapper.Map(request, existingEquipment);
        await _equipmentRepository.UpdateAsync(existingEquipment);
        return _mapper.Map<EquipmentDto>(existingEquipment);
    }

    public async Task<bool> DeleteEquipmentAsync(int id)
    {
        var existingEquipment = await _equipmentRepository.GetByIdAsync(id);
        if (existingEquipment == null)
        {
            return false;
        }

        await _equipmentRepository.DeleteAsync(id);
        return true;
    }

    public async Task<(IEnumerable<EquipmentDto> items, int totalCount)> SearchEquipmentAsync(
        string? keyword,
        string? status,
        string? location,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var result = await _equipmentRepository.SearchAsync(
            keyword,
            status,
            location,
            page,
            pageSize,
            sortBy,
            desc
        );
        return (_mapper.Map<IEnumerable<EquipmentDto>>(result.items), result.totalCount);
    }
}
