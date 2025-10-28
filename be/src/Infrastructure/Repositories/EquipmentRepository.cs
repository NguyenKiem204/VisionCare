using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Equipment;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;
using InfrastructureEquipment = VisionCare.Infrastructure.Models.Equipment;

namespace VisionCare.Infrastructure.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly VisionCareDbContext _context;

    public EquipmentRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Domain.Entities.Equipment>> GetAllAsync()
    {
        var equipment = await _context.Equipment.ToListAsync();
        return equipment.Select(EquipmentMapper.ToDomain);
    }

    public async Task<Domain.Entities.Equipment?> GetByIdAsync(int id)
    {
        var equipment = await _context.Equipment.FirstOrDefaultAsync(e => e.EquipmentId == id);
        return equipment != null ? EquipmentMapper.ToDomain(equipment) : null;
    }

    public async Task<Domain.Entities.Equipment> AddAsync(Domain.Entities.Equipment equipment)
    {
        var equipmentModel = EquipmentMapper.ToInfrastructure(equipment);
        _context.Equipment.Add(equipmentModel);
        await _context.SaveChangesAsync();
        return EquipmentMapper.ToDomain(equipmentModel);
    }

    public async Task UpdateAsync(Domain.Entities.Equipment equipment)
    {
        var existingEquipment = await _context.Equipment.FirstOrDefaultAsync(e => e.EquipmentId == equipment.Id);
        if (existingEquipment != null)
        {
            existingEquipment.Name = equipment.Name;
            existingEquipment.Model = equipment.Model;
            existingEquipment.SerialNumber = equipment.SerialNumber;
            existingEquipment.Manufacturer = equipment.Manufacturer;
            existingEquipment.PurchaseDate = equipment.PurchaseDate;
            existingEquipment.LastMaintenanceDate = equipment.LastMaintenanceDate;
            existingEquipment.Status = equipment.Status;
            existingEquipment.Location = equipment.Location;
            existingEquipment.Notes = equipment.Notes;
            existingEquipment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var equipment = await _context.Equipment.FindAsync(id);
        if (equipment != null)
        {
            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(IEnumerable<Domain.Entities.Equipment> items, int totalCount)> SearchAsync(
        string? keyword,
        string? status,
        string? location,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var query = _context.Equipment.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            var loweredKeyword = keyword.Trim().ToLower();
            query = query.Where(e =>
                e.Name.ToLower().Contains(loweredKeyword) ||
                (e.Model != null && e.Model.ToLower().Contains(loweredKeyword)) ||
                (e.SerialNumber != null && e.SerialNumber.ToLower().Contains(loweredKeyword)) ||
                (e.Manufacturer != null && e.Manufacturer.ToLower().Contains(loweredKeyword))
            );
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(e => e.Status == status);
        }

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(e => e.Location != null && e.Location.Contains(location));
        }

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "name" => desc ? query.OrderByDescending(e => e.Name) : query.OrderBy(e => e.Name),
            "status" => desc ? query.OrderByDescending(e => e.Status) : query.OrderBy(e => e.Status),
            "location" => desc ? query.OrderByDescending(e => e.Location) : query.OrderBy(e => e.Location),
            "purchasedate" => desc ? query.OrderByDescending(e => e.PurchaseDate) : query.OrderBy(e => e.PurchaseDate),
            _ => desc ? query.OrderByDescending(e => e.EquipmentId) : query.OrderBy(e => e.EquipmentId)
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items.Select(EquipmentMapper.ToDomain), totalCount);
    }
}
