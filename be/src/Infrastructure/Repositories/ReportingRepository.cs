using Microsoft.EntityFrameworkCore;
using VisionCare.Application.DTOs.Dashboard;
using VisionCare.Application.Interfaces.Reporting;
using VisionCare.Infrastructure.Data;

namespace VisionCare.Infrastructure.Repositories;

public class ReportingRepository : IReportingRepository
{
    private readonly VisionCareDbContext _db;

    public ReportingRepository(VisionCareDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardSummaryDto> GetAdminSummaryAsync(
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct
    )
    {
        var total = await _db
            .Appointments.Where(a =>
                a.AppointmentDatetime >= fromUtc && a.AppointmentDatetime <= toUtc
            )
            .CountAsync(ct);

        var completed = await _db
            .Appointments.Where(a =>
                a.AppointmentDatetime >= fromUtc
                && a.AppointmentDatetime <= toUtc
                && a.Status == "Completed"
            )
            .CountAsync(ct);

        var canceled = await _db
            .Appointments.Where(a =>
                a.AppointmentDatetime >= fromUtc
                && a.AppointmentDatetime <= toUtc
                && a.Status == "Cancelled"
            )
            .CountAsync(ct);

        var newPatients = await _db.Customers.CountAsync(ct); // TODO: replace with CreatedAt filter if available

        decimal revenue = 0; // TODO: compute from Checkout if available

        return new DashboardSummaryDto
        {
            TotalAppointments = total,
            CompletedAppointments = completed,
            CanceledAppointments = canceled,
            NewPatients = newPatients,
            Revenue = revenue,
        };
    }

    public async Task<IReadOnlyList<TimeSeriesPointDto>> GetAppointmentSeriesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        string bucket,
        int? doctorId,
        CancellationToken ct
    )
    {
        var query = _db
            .Appointments.AsNoTracking()
            .Where(a => a.AppointmentDatetime >= fromUtc && a.AppointmentDatetime <= toUtc);
        if (doctorId.HasValue)
            query = query.Where(a => a.DoctorId == doctorId.Value);

        var data = await query
            .GroupBy(a => new
            {
                y = a.AppointmentDatetime.Year,
                m = a.AppointmentDatetime.Month,
                d = bucket == "day" ? a.AppointmentDatetime.Day : 1,
            })
            .Select(g => new TimeSeriesPointDto
            {
                Label =
                    g.Key.y
                    + "-"
                    + g.Key.m.ToString("00")
                    + (bucket == "day" ? ("-" + g.Key.d.ToString("00")) : ""),
                Count = g.Count(),
            })
            .OrderBy(p => p.Label)
            .ToListAsync(ct);

        return data;
    }

    public async Task<IReadOnlyList<TimeSeriesPointDto>> GetRevenueSeriesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        string bucket,
        int? doctorId,
        CancellationToken ct
    )
    {
        // Placeholder: return empty until Checkout is modeled
        return new List<TimeSeriesPointDto>();
    }

    public async Task<IReadOnlyList<TopServiceDto>> GetTopServicesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        int topN,
        CancellationToken ct
    )
    {
        // TODO: Implement when Service relationship is added to Appointment
        var data = new List<TopServiceDto>();
        return data;
    }

    public async Task<IReadOnlyList<DoctorKpiDto>> GetDoctorKpisAsync(
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken ct
    )
    {
        var data = await _db
            .Appointments.Where(a =>
                a.AppointmentDatetime >= fromUtc
                && a.AppointmentDatetime <= toUtc
                && a.Status == "Completed"
            )
            .GroupBy(a => new { a.DoctorId, a.Doctor.FullName })
            .Select(g => new DoctorKpiDto
            {
                DoctorId = g.Key.DoctorId,
                DoctorName = g.Key.FullName ?? "",
                CompletedCount = g.Count(),
                Utilization = null,
            })
            .OrderByDescending(x => x.CompletedCount)
            .ToListAsync(ct);

        return data;
    }

    public async Task<IReadOnlyList<RecentAppointmentDto>> GetRecentAppointmentsAsync(
        int limit,
        CancellationToken ct
    )
    {
        var data = await _db
            .Appointments.Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .OrderByDescending(a => a.AppointmentDatetime)
            .Take(limit)
            .Select(a => new RecentAppointmentDto
            {
                AppointmentId = a.AppointmentId,
                PatientName = a.Patient.FullName ?? "",
                DoctorName = a.Doctor.FullName ?? "",
                ServiceName = a.ServiceDetail.Service.Name ?? "",
                AppointmentDate = a.AppointmentDatetime,
                Status = a.Status ?? "",
                Notes = a.Notes,
            })
            .ToListAsync(ct);

        return data;
    }
}
