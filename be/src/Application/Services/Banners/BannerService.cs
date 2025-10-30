using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.DTOs.BannerDto;
using VisionCare.Application.Interfaces.Banners;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Banners;

public class BannerService : IBannerService
{
    private readonly IBannerRepository _repo;
    public BannerService(IBannerRepository repo) { _repo = repo; }

    public async Task<IReadOnlyList<BannerDto>> GetByPlaceAsync(string place, CancellationToken ct = default)
    {
        var list = await _repo.GetByPlaceAsync(place, ct);
        return list.Select(ToDto).ToList();
    }

    public async Task<BannerDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        return entity == null ? null : ToDto(entity);
    }

    public async Task CreateAsync(CreateBannerRequest request, CancellationToken ct = default)
    {
        var entity = new Banner
        {
            Title = request.Title,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            LinkUrl = request.LinkUrl,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };
        await _repo.CreateAsync(entity, ct);
    }

    public async Task UpdateAsync(int id, UpdateBannerRequest request, CancellationToken ct = default)
    {
        var entity = new Banner
        {
            BannerId = id,
            Title = request.Title,
            Description = request.Description,
            ImageUrl = request.ImageUrl,
            LinkUrl = request.LinkUrl,
            DisplayOrder = request.DisplayOrder,
            Status = request.Status,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };
        await _repo.UpdateAsync(entity, ct);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default)
        => _repo.DeleteAsync(id, ct);

    private static BannerDto ToDto(Banner b) => new()
    {
        BannerId = b.BannerId,
        Title = b.Title,
        Description = b.Description,
        ImageUrl = b.ImageUrl,
        LinkUrl = b.LinkUrl,
        DisplayOrder = b.DisplayOrder,
        Status = b.Status,
        StartDate = b.StartDate,
        EndDate = b.EndDate,
    };
}


