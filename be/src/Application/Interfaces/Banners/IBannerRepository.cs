using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Banners;

public interface IBannerRepository
{
    Task<IReadOnlyList<Banner>> GetByPlaceAsync(string place, CancellationToken ct = default);
    Task<Banner?> GetByIdAsync(int id, CancellationToken ct = default);
    Task CreateAsync(Banner banner, CancellationToken ct = default);
    Task UpdateAsync(Banner banner, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}


