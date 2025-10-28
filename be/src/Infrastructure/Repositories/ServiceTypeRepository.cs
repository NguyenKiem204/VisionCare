using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.ServiceTypes;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;
using InfrastructureServiceType = VisionCare.Infrastructure.Models.Servicestype;

namespace VisionCare.Infrastructure.Repositories;

public class ServiceTypeRepository : IServiceTypeRepository
{
    private readonly VisionCareDbContext _context;

    public ServiceTypeRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServiceType>> GetAllAsync()
    {
        var serviceTypes = await _context.Servicestypes.ToListAsync();
        return serviceTypes.Select(ServiceTypeMapper.ToDomain);
    }

    public async Task<ServiceType?> GetByIdAsync(int id)
    {
        var serviceType = await _context.Servicestypes.FirstOrDefaultAsync(st => st.ServiceTypeId == id);
        return serviceType != null ? ServiceTypeMapper.ToDomain(serviceType) : null;
    }

    public async Task<ServiceType> AddAsync(ServiceType serviceType)
    {
        var serviceTypeModel = ServiceTypeMapper.ToInfrastructure(serviceType);
        _context.Servicestypes.Add(serviceTypeModel);
        await _context.SaveChangesAsync();
        return ServiceTypeMapper.ToDomain(serviceTypeModel);
    }

    public async Task UpdateAsync(ServiceType serviceType)
    {
        var existingServiceType = await _context.Servicestypes.FirstOrDefaultAsync(st => st.ServiceTypeId == serviceType.Id);
        if (existingServiceType != null)
        {
            existingServiceType.Name = serviceType.Name;
            existingServiceType.DurationMinutes = (short)serviceType.DurationMinutes;
            existingServiceType.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var serviceType = await _context.Servicestypes.FindAsync(id);
        if (serviceType != null)
        {
            _context.Servicestypes.Remove(serviceType);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(IEnumerable<ServiceType> items, int totalCount)> SearchAsync(
        string? keyword,
        int? minDuration,
        int? maxDuration,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var query = _context.Servicestypes.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            var loweredKeyword = keyword.Trim().ToLower();
            query = query.Where(st => st.Name.ToLower().Contains(loweredKeyword));
        }

        if (minDuration.HasValue)
        {
            query = query.Where(st => st.DurationMinutes >= minDuration.Value);
        }

        if (maxDuration.HasValue)
        {
            query = query.Where(st => st.DurationMinutes <= maxDuration.Value);
        }

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "name" => desc ? query.OrderByDescending(st => st.Name) : query.OrderBy(st => st.Name),
            "duration" => desc ? query.OrderByDescending(st => st.DurationMinutes) : query.OrderBy(st => st.DurationMinutes),
            _ => desc ? query.OrderByDescending(st => st.ServiceTypeId) : query.OrderBy(st => st.ServiceTypeId)
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items.Select(ServiceTypeMapper.ToDomain), totalCount);
    }
}
