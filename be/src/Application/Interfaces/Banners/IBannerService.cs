using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VisionCare.Application.DTOs.BannerDto;

namespace VisionCare.Application.Interfaces.Banners;

public interface IBannerService
{
    Task<IReadOnlyList<BannerDto>> GetByPlaceAsync(string place, CancellationToken ct = default);
    Task<BannerDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task CreateAsync(CreateBannerRequest request, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateBannerRequest request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}


