using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.Interfaces.Content;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class SectionContentRepository : ISectionContentRepository
{
    private readonly VisionCareDbContext _db;
    public SectionContentRepository(VisionCareDbContext db) { _db = db; }

    public async Task<SectionContent?> GetByKeyAsync(string key, CancellationToken ct = default)
    {
        var model = await _db.Sectioncontents.AsNoTracking().FirstOrDefaultAsync(x => x.SectionKey == key, ct);
        return model == null ? null : SectionContentMapper.ToDomain(model);
    }

    public async Task<IReadOnlyList<SectionContent>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _db.Sectioncontents.AsNoTracking().ToListAsync(ct);
        return list.Select(SectionContentMapper.ToDomain).ToList();
    }

    public async Task CreateAsync(SectionContent section, CancellationToken ct = default)
    {
        var model = SectionContentMapper.ToInfrastructure(section);
        _db.Sectioncontents.Add(model);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(SectionContent section, CancellationToken ct = default)
    {
        var model = await _db.Sectioncontents.FirstOrDefaultAsync(x => x.SectionKey == section.SectionKey, ct);
        if (model == null) return;
        model.Content = section.Content;
        model.ImageUrl = section.ImageUrl;
        model.MoreData = section.MoreData;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(string key, CancellationToken ct = default)
    {
        var model = await _db.Sectioncontents.FirstOrDefaultAsync(x => x.SectionKey == key, ct);
        if (model == null) return;
        _db.Sectioncontents.Remove(model);
        await _db.SaveChangesAsync(ct);
    }
}


