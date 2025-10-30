using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.Interfaces.Banners;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class BannerRepository : IBannerRepository
{
    private readonly VisionCareDbContext _db;
    public BannerRepository(VisionCareDbContext db) { _db = db; }

    public async Task<IReadOnlyList<Banner>> GetByPlaceAsync(string place, CancellationToken ct = default)
    {
        // Place not in schema yet; return all ordered
        var list = await _db.Banners.AsNoTracking().OrderBy(b => b.DisplayOrder).ToListAsync(ct);
        return list.Select(BannerMapper.ToDomain).ToList();
    }

    public async Task<Banner?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var model = await _db.Banners.AsNoTracking().FirstOrDefaultAsync(x => x.BannerId == id, ct);
        return model == null ? null : BannerMapper.ToDomain(model);
    }

    public async Task CreateAsync(Banner banner, CancellationToken ct = default)
    {
        var model = BannerMapper.ToInfrastructure(banner);
        _db.Banners.Add(model);
        await _db.SaveChangesAsync(ct);
        banner.BannerId = model.BannerId;
    }

    public async Task UpdateAsync(Banner banner, CancellationToken ct = default)
    {
        var model = await _db.Banners.FirstOrDefaultAsync(x => x.BannerId == banner.BannerId, ct);
        if (model == null) return;
        model.Title = banner.Title;
        model.Description = banner.Description;
        model.ImageUrl = banner.ImageUrl;
        model.LinkUrl = banner.LinkUrl;
        model.DisplayOrder = banner.DisplayOrder;
        model.Status = banner.Status;
        model.StartDate = banner.StartDate;
        model.EndDate = banner.EndDate;
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var model = await _db.Banners.FirstOrDefaultAsync(x => x.BannerId == id, ct);
        if (model == null) return;
        _db.Banners.Remove(model);
        await _db.SaveChangesAsync(ct);
    }
}


