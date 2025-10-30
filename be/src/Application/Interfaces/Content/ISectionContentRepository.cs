using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Content;

public interface ISectionContentRepository
{
    Task<SectionContent?> GetByKeyAsync(string key, CancellationToken ct = default);
    Task<IReadOnlyList<SectionContent>> GetAllAsync(CancellationToken ct = default);
    Task CreateAsync(SectionContent section, CancellationToken ct = default);
    Task UpdateAsync(SectionContent section, CancellationToken ct = default);
    Task DeleteAsync(string key, CancellationToken ct = default);
}


