using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;

namespace VisionCare.Infrastructure.Repositories;

public class SlotRepository : ISlotRepository
{
    private readonly VisionCareDbContext _context;

    public SlotRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Slot>> GetAllAsync()
    {
        var slots = await _context.Slots.Include(s => s.ServiceType).ToListAsync();

        return slots.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<VisionCare.Domain.Entities.Slot?> GetByIdAsync(int id)
    {
        var slot = await _context
            .Slots.Include(s => s.ServiceType)
            .FirstOrDefaultAsync(s => s.SlotId == id);

        return slot != null ? ConvertToDomainEntity(slot) : null;
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Slot>> GetByServiceTypeAsync(
        int serviceTypeId
    )
    {
        var slots = await _context
            .Slots.Include(s => s.ServiceType)
            .Where(s => s.ServiceTypeId == serviceTypeId)
            .ToListAsync();

        return slots.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Slot>> GetAvailableSlotsAsync(
        int doctorId,
        DateOnly scheduleDate,
        int? serviceTypeId
    )
    {
        var query = _context
            .Slots.Include(s => s.ServiceType)
            .Include(s => s.Schedules)
            .Where(s =>
                !s.Schedules.Any(sch =>
                    sch.DoctorId == doctorId
                    && sch.ScheduleDate == scheduleDate
                    && sch.Status == "Booked"
                )
            )
            .AsQueryable();

        if (serviceTypeId.HasValue)
        {
            query = query.Where(s => s.ServiceTypeId == serviceTypeId);
        }

        var slots = await query.ToListAsync();
        return slots.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<VisionCare.Domain.Entities.Slot> AddAsync(
        VisionCare.Domain.Entities.Slot slot
    )
    {
        var infrastructureModel = ConvertToInfrastructureModel(slot);
        _context.Slots.Add(infrastructureModel);
        await _context.SaveChangesAsync();

        // Return the domain entity with updated ID
        slot.Id = infrastructureModel.SlotId;
        return slot;
    }

    public async Task UpdateAsync(VisionCare.Domain.Entities.Slot slot)
    {
        var existingModel = await _context.Slots.FindAsync(slot.Id);
        if (existingModel != null)
        {
            UpdateInfrastructureModel(existingModel, slot);
            _context.Slots.Update(existingModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var slot = await _context.Slots.FindAsync(id);
        if (slot != null)
        {
            _context.Slots.Remove(slot);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Slots.AnyAsync(s => s.SlotId == id);
    }

    public async Task<bool> TimeSlotExistsAsync(
        TimeOnly startTime,
        TimeOnly endTime,
        int serviceTypeId,
        int? excludeId = null
    )
    {
        var query = _context.Slots.Where(s =>
            s.StartTime == startTime && s.EndTime == endTime && s.ServiceTypeId == serviceTypeId
        );

        if (excludeId.HasValue)
        {
            query = query.Where(s => s.SlotId != excludeId);
        }

        return await query.AnyAsync();
    }

    public async Task<int> DeleteByServiceTypeAndTimeRangeAsync(int serviceTypeId, TimeOnly startInclusive, TimeOnly endInclusive)
    {
        var toDelete = await _context.Slots
            .Where(s => s.ServiceTypeId == serviceTypeId && s.StartTime >= startInclusive && s.EndTime <= endInclusive)
            .ToListAsync();
        var count = toDelete.Count;
        if (count > 0)
        {
            _context.Slots.RemoveRange(toDelete);
            await _context.SaveChangesAsync();
        }
        return count;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Slots.CountAsync();
    }

    public async Task<IEnumerable<VisionCare.Domain.Entities.Slot>> GetPagedAsync(
        int page,
        int pageSize
    )
    {
        var slots = await _context
            .Slots.Include(s => s.ServiceType)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return slots.Select(ConvertToDomainEntity).ToList();
    }

    private static VisionCare.Domain.Entities.Slot ConvertToDomainEntity(
        Infrastructure.Models.Slot model
    )
    {
        return new VisionCare.Domain.Entities.Slot
        {
            Id = model.SlotId,
            StartTime = model.StartTime,
            EndTime = model.EndTime,
            ServiceTypeId = model.ServiceTypeId,
            Created = DateTime.UtcNow, // Slots don't have created/updated timestamps in DB
            LastModified = DateTime.UtcNow,
        };
    }

    private static Infrastructure.Models.Slot ConvertToInfrastructureModel(
        VisionCare.Domain.Entities.Slot domainEntity
    )
    {
        return new Infrastructure.Models.Slot
        {
            SlotId = domainEntity.Id,
            StartTime = domainEntity.StartTime,
            EndTime = domainEntity.EndTime,
            ServiceTypeId = domainEntity.ServiceTypeId,
        };
    }

    private static void UpdateInfrastructureModel(
        Infrastructure.Models.Slot model,
        VisionCare.Domain.Entities.Slot domainEntity
    )
    {
        model.StartTime = domainEntity.StartTime;
        model.EndTime = domainEntity.EndTime;
        model.ServiceTypeId = domainEntity.ServiceTypeId;
    }
}
