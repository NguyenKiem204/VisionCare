using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.DTOs.SectionContentDto;
using VisionCare.Application.Interfaces.Content;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Content;

public class SectionContentService : ISectionContentService
{
    private readonly ISectionContentRepository _repo;
    public SectionContentService(ISectionContentRepository repo) { _repo = repo; }

    public async Task<SectionContentDto?> GetByKeyAsync(string key, CancellationToken ct = default)
    {
        var entity = await _repo.GetByKeyAsync(key, ct);
        if (entity == null) return null;
        return ToDto(entity);
    }

    public async Task<IReadOnlyList<SectionContentDto>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _repo.GetAllAsync(ct);
        return list.Select(ToDto).ToList();
    }

    public async Task CreateAsync(SectionContentUpsertDto request, CancellationToken ct = default)
    {
        var entity = new SectionContent
        {
            SectionKey = request.SectionKey,
            Content = request.Content,
            ImageUrl = request.ImageUrl,
            MoreData = request.MoreData
        };
        await _repo.CreateAsync(entity, ct);
    }

    public async Task UpdateAsync(string key, SectionContentUpsertDto request, CancellationToken ct = default)
    {
        var entity = new SectionContent
        {
            SectionKey = key,
            Content = request.Content,
            ImageUrl = request.ImageUrl,
            MoreData = request.MoreData
        };
        await _repo.UpdateAsync(entity, ct);
    }

    public Task DeleteAsync(string key, CancellationToken ct = default)
        => _repo.DeleteAsync(key, ct);

    private static SectionContentDto ToDto(SectionContent e) => new()
    {
        SectionKey = e.SectionKey,
        Content = e.Content,
        ImageUrl = e.ImageUrl,
        MoreData = e.MoreData
    };
}


