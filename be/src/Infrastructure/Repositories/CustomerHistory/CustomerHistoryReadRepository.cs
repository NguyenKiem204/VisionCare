using Microsoft.EntityFrameworkCore;
using VisionCare.Application.DTOs.CustomerHistory;
using VisionCare.Application.Interfaces.CustomerHistory;
using VisionCare.Infrastructure.Data;

namespace VisionCare.Infrastructure.Repositories.CustomerHistory;

public class CustomerHistoryReadRepository : ICustomerHistoryReadRepository
{
    private readonly VisionCareDbContext _db;

    public CustomerHistoryReadRepository(VisionCareDbContext db)
    {
        _db = db;
    }

    public async Task<(IEnumerable<CustomerBookingHistoryDto> items, int totalCount)> GetBookingsAsync(
        int accountId,
        CustomerBookingHistoryQuery query
    )
    {
        var bookingsQuery = _db
            .Appointments.AsNoTracking()
            .Include(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Include(a => a.ServiceDetail)
            .ThenInclude(sd => sd.ServiceType)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Where(a => a.PatientId == accountId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            bookingsQuery = bookingsQuery.Where(a =>
                a.Status != null && a.Status.ToLower() == query.Status!.Trim().ToLower()
            );
        }

        var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        if (query.UpcomingOnly && !query.PastOnly)
        {
            bookingsQuery = bookingsQuery.Where(a => a.AppointmentDatetime >= now);
        }
        else if (query.PastOnly && !query.UpcomingOnly)
        {
            bookingsQuery = bookingsQuery.Where(a => a.AppointmentDatetime < now);
        }

        bookingsQuery = (query.SortBy?.ToLowerInvariant()) switch
        {
            "status" => query.SortDescending
                ? bookingsQuery.OrderByDescending(a => a.Status)
                : bookingsQuery.OrderBy(a => a.Status),
            "created" => query.SortDescending
                ? bookingsQuery.OrderByDescending(a => a.CreatedAt)
                : bookingsQuery.OrderBy(a => a.CreatedAt),
            "paymentstatus" => query.SortDescending
                ? bookingsQuery.OrderByDescending(a => a.PaymentStatus)
                : bookingsQuery.OrderBy(a => a.PaymentStatus),
            _ => query.SortDescending
                ? bookingsQuery.OrderByDescending(a => a.AppointmentDatetime)
                : bookingsQuery.OrderBy(a => a.AppointmentDatetime),
        };

        var totalCount = await bookingsQuery.CountAsync();

        var skip = (query.Page - 1) * query.PageSize;

        var items = await bookingsQuery
            .Skip(skip)
            .Take(query.PageSize)
            .Select(a => new CustomerBookingHistoryDto
            {
                AppointmentId = a.AppointmentId,
                AppointmentCode = a.AppointmentCode ?? string.Empty,
                AppointmentDate = a.AppointmentDatetime,
                Status = a.Status ?? "Pending",
                PaymentStatus = a.PaymentStatus ?? "Unpaid",
                TotalAmount = a.ActualCost,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor != null ? a.Doctor.FullName : string.Empty,
                DoctorAvatar = a.Doctor != null ? a.Doctor.Avatar : null,
                DoctorSpecialization = a.Doctor != null && a.Doctor.Specialization != null
                    ? a.Doctor.Specialization.Name
                    : null,
                ServiceId = a.ServiceDetail != null ? a.ServiceDetail.ServiceId : 0,
                ServiceDetailId = a.ServiceDetailId,
                ServiceName = a.ServiceDetail != null && a.ServiceDetail.Service != null
                    ? a.ServiceDetail.Service.Name
                    : string.Empty,
                ServiceTypeName = a.ServiceDetail != null && a.ServiceDetail.ServiceType != null
                    ? a.ServiceDetail.ServiceType.Name
                    : string.Empty,
                Notes = a.Notes,
                CreatedAt = a.CreatedAt ?? a.AppointmentDatetime,
            })
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<CustomerPrescriptionHistoryDto>> GetPrescriptionsAsync(
        int accountId,
        int? encounterId = null
    )
    {
        var prescriptionsQuery = _db
            .Prescriptions.AsNoTracking()
            .Include(p => p.Prescriptionlines)
            .Include(p => p.Encounter)
            .ThenInclude(e => e.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(p => p.Encounter)
            .ThenInclude(e => e.Appointment)
            .Where(p => p.Encounter.CustomerId == accountId)
            .AsQueryable();

        if (encounterId.HasValue)
        {
            prescriptionsQuery = prescriptionsQuery.Where(p => p.EncounterId == encounterId.Value);
        }

        var list = await prescriptionsQuery
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new CustomerPrescriptionHistoryDto
            {
                PrescriptionId = p.PrescriptionId,
                EncounterId = p.EncounterId,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Notes = p.Notes,
                EncounterStatus = p.Encounter != null ? p.Encounter.Status : string.Empty,
                EncounterDate = p.Encounter != null ? p.Encounter.CreatedAt : p.CreatedAt,
                DoctorId = p.Encounter != null ? p.Encounter.DoctorId : 0,
                DoctorName = p.Encounter != null && p.Encounter.Doctor != null
                    ? p.Encounter.Doctor.FullName
                    : string.Empty,
                DoctorAvatar = p.Encounter != null && p.Encounter.Doctor != null
                    ? p.Encounter.Doctor.Avatar
                    : null,
                DoctorSpecialization = p.Encounter != null
                        && p.Encounter.Doctor != null
                        && p.Encounter.Doctor.Specialization != null
                    ? p.Encounter.Doctor.Specialization.Name
                    : null,
                Lines = p.Prescriptionlines != null
                    ? p.Prescriptionlines.Select(l => new CustomerPrescriptionLineDto
                    {
                        LineId = l.LineId,
                        DrugName = l.DrugName,
                        DrugCode = l.DrugCode,
                        Dosage = l.Dosage,
                        Frequency = l.Frequency,
                        Duration = l.Duration,
                        Instructions = l.Instructions,
                    })
                    : Array.Empty<CustomerPrescriptionLineDto>(),
            })
            .ToListAsync();

        return list;
    }

    public async Task<IEnumerable<CustomerMedicalHistoryDto>> GetMedicalHistoryAsync(int accountId)
    {
        var histories = await _db
            .Medicalhistories.AsNoTracking()
            .Include(m => m.Appointment)
            .ThenInclude(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(m => m.Appointment)
            .ThenInclude(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Where(m => m.Appointment.PatientId == accountId)
            .OrderByDescending(m =>
                m.CreatedAt ?? (DateTime?)(m.Appointment != null ? m.Appointment.AppointmentDatetime : DateTime.MinValue)
            )
            .Select(m => new CustomerMedicalHistoryDto
            {
                MedicalHistoryId = m.MedicalHistoryId,
                AppointmentId = m.AppointmentId,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt,
                Diagnosis = m.Diagnosis,
                Symptoms = m.Symptoms,
                Treatment = m.Treatment,
                Prescription = m.Prescription,
                VisionLeft = m.VisionLeft,
                VisionRight = m.VisionRight,
                AdditionalTests = m.AdditionalTests,
                Notes = m.Notes,
                AppointmentDate = m.Appointment != null ? m.Appointment.AppointmentDatetime : null,
                AppointmentStatus = m.Appointment != null ? (m.Appointment.Status ?? "Pending") : "Pending",
                DoctorId = m.Appointment != null ? m.Appointment.DoctorId : 0,
                DoctorName = m.Appointment != null && m.Appointment.Doctor != null
                    ? m.Appointment.Doctor.FullName
                    : string.Empty,
                DoctorAvatar = m.Appointment != null && m.Appointment.Doctor != null
                    ? m.Appointment.Doctor.Avatar
                    : null,
                ServiceName = m.Appointment != null
                        && m.Appointment.ServiceDetail != null
                        && m.Appointment.ServiceDetail.Service != null
                    ? m.Appointment.ServiceDetail.Service.Name
                    : string.Empty,
            })
            .ToListAsync();

        return histories;
    }
}

