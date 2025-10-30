using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.DTOs.SectionContentDto;

namespace VisionCare.Application.Interfaces.Content;

public interface ISectionContentService
{
    Task<SectionContentDto?> GetByKeyAsync(string key, CancellationToken ct = default);
    Task<IReadOnlyList<SectionContentDto>> GetAllAsync(CancellationToken ct = default);
    Task CreateAsync(SectionContentUpsertDto request, CancellationToken ct = default);
    Task UpdateAsync(string key, SectionContentUpsertDto request, CancellationToken ct = default);
    Task DeleteAsync(string key, CancellationToken ct = default);
}


