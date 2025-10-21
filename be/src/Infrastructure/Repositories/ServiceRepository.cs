using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Domain.Entities; // for Specialization and ServiceDetail
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Models;
using DomainService = VisionCare.Domain.Entities.Service;

namespace VisionCare.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly VisionCareDbContext _context;

    public ServiceRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DomainService>> GetAllAsync()
    {
        var services = await _context
            .Services.Include(s => s.Specialization)
            .Include(s => s.Servicesdetails)
            .ThenInclude(sd => sd.ServiceType)
            .ToListAsync();

        return services.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<DomainService?> GetByIdAsync(int id)
    {
        var service = await _context
            .Services.Include(s => s.Specialization)
            .Include(s => s.Servicesdetails)
            .ThenInclude(sd => sd.ServiceType)
            .FirstOrDefaultAsync(s => s.ServiceId == id);

        return service != null ? ConvertToDomainEntity(service) : null;
    }

    public async Task<DomainService?> GetByNameAsync(string name)
    {
        var service = await _context
            .Services.Include(s => s.Specialization)
            .Include(s => s.Servicesdetails)
            .ThenInclude(sd => sd.ServiceType)
            .FirstOrDefaultAsync(s => s.Name == name);

        return service != null ? ConvertToDomainEntity(service) : null;
    }

    public async Task<IEnumerable<DomainService>> GetBySpecializationAsync(int specializationId)
    {
        var services = await _context
            .Services.Include(s => s.Specialization)
            .Include(s => s.Servicesdetails)
            .ThenInclude(sd => sd.ServiceType)
            .Where(s => s.SpecializationId == specializationId)
            .ToListAsync();

        return services.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<DomainService>> GetActiveAsync()
    {
        var services = await _context
            .Services.Include(s => s.Specialization)
            .Include(s => s.Servicesdetails)
            .ThenInclude(sd => sd.ServiceType)
            .Where(s => s.Status == "Active")
            .ToListAsync();

        return services.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<DomainService>> SearchAsync(
        string keyword,
        int? specializationId,
        string? status
    )
    {
        var query = _context
            .Services.Include(s => s.Specialization)
            .Include(s => s.Servicesdetails)
            .ThenInclude(sd => sd.ServiceType)
            .AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(s =>
                s.Name.Contains(keyword)
                || (s.Description != null && s.Description.Contains(keyword))
            );
        }

        if (specializationId.HasValue)
        {
            query = query.Where(s => s.SpecializationId == specializationId);
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(s => s.Status == status);
        }

        var services = await query.ToListAsync();
        return services.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<DomainService> AddAsync(DomainService service)
    {
        var infrastructureModel = ConvertToInfrastructureModel(service);
        _context.Services.Add(infrastructureModel);
        await _context.SaveChangesAsync();

        // Return the domain entity with updated ID
        service.Id = infrastructureModel.ServiceId;
        return service;
    }

    public async Task UpdateAsync(DomainService service)
    {
        var existingModel = await _context.Services.FindAsync(service.Id);
        if (existingModel != null)
        {
            UpdateInfrastructureModel(existingModel, service);
            _context.Services.Update(existingModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var service = await _context.Services.FindAsync(id);
        if (service != null)
        {
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Services.AnyAsync(s => s.ServiceId == id);
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
    {
        var query = _context.Services.Where(s => s.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(s => s.ServiceId != excludeId);
        }

        return await query.AnyAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Services.CountAsync();
    }

    public async Task<IEnumerable<DomainService>> GetPagedAsync(int page, int pageSize)
    {
        var services = await _context
            .Services.Include(s => s.Specialization)
            .Include(s => s.Servicesdetails)
            .ThenInclude(sd => sd.ServiceType)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return services.Select(ConvertToDomainEntity).ToList();
    }

    private static DomainService ConvertToDomainEntity(Infrastructure.Models.Service model)
    {
        return new DomainService
        {
            Id = model.ServiceId,
            Name = model.Name ?? string.Empty,
            Description = model.Description,
            Benefits = model.Benefits,
            Status = model.Status ?? "Active",
            SpecializationId = model.SpecializationId,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            Specialization =
                model.Specialization != null
                    ? new VisionCare.Domain.Entities.Specialization
                    {
                        Id = model.Specialization.SpecializationId,
                        SpecializationName = model.Specialization.Name,
                    }
                    : null,
            ServiceDetails =
                model
                    .Servicesdetails?.Select(sd => new VisionCare.Domain.Entities.ServiceDetail
                    {
                        Id = sd.ServiceDetailId,
                        ServiceId = sd.ServiceId,
                        ServiceTypeId = sd.ServiceTypeId,
                        Cost = sd.Cost,
                        ServiceType =
                            sd.ServiceType != null
                                ? new VisionCare.Domain.Entities.ServiceType
                                {
                                    Id = sd.ServiceType.ServiceTypeId,
                                    Name = sd.ServiceType.Name,
                                    DurationMinutes = sd.ServiceType.DurationMinutes,
                                }
                                : new VisionCare.Domain.Entities.ServiceType(),
                    })
                    .ToList() ?? new List<VisionCare.Domain.Entities.ServiceDetail>(),
        };
    }

    private static Infrastructure.Models.Service ConvertToInfrastructureModel(
        DomainService domainEntity
    )
    {
        return new Infrastructure.Models.Service
        {
            ServiceId = domainEntity.Id,
            Name = domainEntity.Name,
            Description = domainEntity.Description,
            Benefits = domainEntity.Benefits,
            Status = domainEntity.Status,
            SpecializationId = domainEntity.SpecializationId,
            // CreatedAt/UpdatedAt not present in Infrastructure model
        };
    }

    private static void UpdateInfrastructureModel(
        Infrastructure.Models.Service model,
        DomainService domainEntity
    )
    {
        model.Name = domainEntity.Name;
        model.Description = domainEntity.Description;
        model.Benefits = domainEntity.Benefits;
        model.Status = domainEntity.Status;
        model.SpecializationId = domainEntity.SpecializationId;
        // No UpdatedAt field on Infrastructure model; ignore
    }
}
