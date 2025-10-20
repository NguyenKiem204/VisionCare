using Microsoft.Extensions.Caching.Memory;
using VisionCare.Application.DTOs.Dashboard;
using VisionCare.Application.Interfaces.Reporting;

namespace VisionCare.Application.Services.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IReportingRepository _reportingRepository;
    private readonly IMemoryCache _cache;

    public DashboardService(IReportingRepository reportingRepository, IMemoryCache cache)
    {
        _reportingRepository = reportingRepository;
        _cache = cache;
    }

    public async Task<DashboardSummaryDto> GetAdminSummaryAsync(
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct
    )
    {
        var key = ($"admin-summary:{fromUtc:o}:{toUtc:o}");
        if (_cache.TryGetValue(key, out DashboardSummaryDto? cached) && cached != null)
            return cached;

        var data = await _reportingRepository.GetAdminSummaryAsync(fromUtc, toUtc, ct);
        _cache.Set(key, data, TimeSpan.FromSeconds(60));
        return data;
    }

    public async Task<DashboardSummaryDto> GetDoctorSummaryAsync(
        int doctorId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct
    )
    {
        var series = await _reportingRepository.GetAppointmentSeriesAsync(
            fromUtc,
            toUtc,
            "day",
            doctorId,
            ct
        );
        var total = series.Sum(p => p.Count);
        return new DashboardSummaryDto { TotalAppointments = total };
    }

    public async Task<IReadOnlyList<TimeSeriesPointDto>> GetAppointmentSeriesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        string bucket,
        int? doctorId,
        CancellationToken ct
    )
    {
        var key = ($"apts-series:{doctorId}:{bucket}:{fromUtc:o}:{toUtc:o}");
        if (
            _cache.TryGetValue(key, out IReadOnlyList<TimeSeriesPointDto>? cached)
            && cached != null
        )
            return cached;

        var data = await _reportingRepository.GetAppointmentSeriesAsync(
            fromUtc,
            toUtc,
            bucket,
            doctorId,
            ct
        );
        _cache.Set(key, data, TimeSpan.FromSeconds(60));
        return data;
    }

    public Task<IReadOnlyList<TopServiceDto>> GetTopServicesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        int topN,
        CancellationToken ct
    ) => _reportingRepository.GetTopServicesAsync(fromUtc, toUtc, topN, ct);

    public Task<IReadOnlyList<DoctorKpiDto>> GetDoctorKpisAsync(
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct
    ) => _reportingRepository.GetDoctorKpisAsync(fromUtc, toUtc, ct);

    public Task<IReadOnlyList<RecentAppointmentDto>> GetRecentAppointmentsAsync(
        int limit,
        CancellationToken ct
    ) => _reportingRepository.GetRecentAppointmentsAsync(limit, ct);
}
