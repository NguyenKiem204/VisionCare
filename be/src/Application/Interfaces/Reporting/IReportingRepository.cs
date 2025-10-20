using VisionCare.Application.DTOs.Dashboard;

namespace VisionCare.Application.Interfaces.Reporting;

public interface IReportingRepository
{
    Task<DashboardSummaryDto> GetAdminSummaryAsync(
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct
    );
    Task<IReadOnlyList<TimeSeriesPointDto>> GetAppointmentSeriesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        string bucket,
        int? doctorId,
        CancellationToken ct
    );
    Task<IReadOnlyList<TimeSeriesPointDto>> GetRevenueSeriesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        string bucket,
        int? doctorId,
        CancellationToken ct
    );
    Task<IReadOnlyList<TopServiceDto>> GetTopServicesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        int topN,
        CancellationToken ct
    );
    Task<IReadOnlyList<DoctorKpiDto>> GetDoctorKpisAsync(
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct
    );
    Task<IReadOnlyList<RecentAppointmentDto>> GetRecentAppointmentsAsync(
        int limit,
        CancellationToken ct
    );
}
