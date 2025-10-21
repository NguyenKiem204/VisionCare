using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;
using DomainAppointment = VisionCare.Domain.Entities.Appointment;

namespace VisionCare.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly VisionCareDbContext _context;

    public AppointmentRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DomainAppointment>> GetAllAsync()
    {
        var appointments = await _context
            .Appointments.Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(a => a.Patient)
            .ToListAsync();

        return appointments.Select(AppointmentMapper.ToDomain);
    }

    public async Task<DomainAppointment?> GetByIdAsync(int id)
    {
        var appointment = await _context
            .Appointments.Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.AppointmentId == id);

        return appointment != null ? AppointmentMapper.ToDomain(appointment) : null;
    }

    public async Task<DomainAppointment> AddAsync(DomainAppointment appointment)
    {
        var appointmentModel = AppointmentMapper.ToInfrastructure(appointment);
        _context.Appointments.Add(appointmentModel);
        await _context.SaveChangesAsync();
        return AppointmentMapper.ToDomain(appointmentModel);
    }

    public async Task UpdateAsync(DomainAppointment appointment)
    {
        var existingAppointment = await _context.Appointments.FirstOrDefaultAsync(a =>
            a.AppointmentId == appointment.Id
        );

        if (existingAppointment != null)
        {
            existingAppointment.AppointmentDatetime =
                appointment.AppointmentDate ?? existingAppointment.AppointmentDatetime;
            existingAppointment.Status = appointment.AppointmentStatus;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<DomainAppointment>> GetByDoctorAsync(
        int doctorId,
        DateTime? date = null
    )
    {
        var query = _context
            .Appointments.Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctorId)
            .AsQueryable();

        if (date.HasValue)
        {
            var startOfDay = date.Value.Date;
            var endOfDay = startOfDay.AddDays(1);
            query = query.Where(a =>
                a.AppointmentDatetime >= startOfDay && a.AppointmentDatetime < endOfDay
            );
        }

        var appointments = await query.ToListAsync();
        return appointments.Select(AppointmentMapper.ToDomain);
    }

    public async Task<IEnumerable<DomainAppointment>> GetByCustomerAsync(
        int customerId,
        DateTime? date = null
    )
    {
        var query = _context
            .Appointments.Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(a => a.Patient)
            .Where(a => a.PatientId == customerId)
            .AsQueryable();

        if (date.HasValue)
        {
            var startOfDay = date.Value.Date;
            var endOfDay = startOfDay.AddDays(1);
            query = query.Where(a =>
                a.AppointmentDatetime >= startOfDay && a.AppointmentDatetime < endOfDay
            );
        }

        var appointments = await query.ToListAsync();
        return appointments.Select(AppointmentMapper.ToDomain);
    }

    public async Task<IEnumerable<DomainAppointment>> GetByDateAsync(DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        var appointments = await _context
            .Appointments.Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(a => a.Patient)
            .Where(a => a.AppointmentDatetime >= startOfDay && a.AppointmentDatetime < endOfDay)
            .ToListAsync();

        return appointments.Select(AppointmentMapper.ToDomain);
    }

    public async Task<IEnumerable<DomainAppointment>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate
    )
    {
        var appointments = await _context
            .Appointments.Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(a => a.Patient)
            .Where(a => a.AppointmentDatetime >= startDate && a.AppointmentDatetime <= endDate)
            .ToListAsync();

        return appointments.Select(AppointmentMapper.ToDomain);
    }

    public async Task<IEnumerable<DomainAppointment>> GetUpcomingAsync(
        int? doctorId = null,
        int? customerId = null
    )
    {
        var query = _context
            .Appointments.Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(a => a.Patient)
            .Where(a => a.AppointmentDatetime > DateTime.UtcNow)
            .AsQueryable();

        if (doctorId.HasValue)
        {
            query = query.Where(a => a.DoctorId == doctorId.Value);
        }

        if (customerId.HasValue)
        {
            query = query.Where(a => a.PatientId == customerId.Value);
        }

        var appointments = await query.OrderBy(a => a.AppointmentDatetime).ToListAsync();
        return appointments.Select(AppointmentMapper.ToDomain);
    }

    public async Task<IEnumerable<DomainAppointment>> GetOverdueAsync()
    {
        var appointments = await _context
            .Appointments.Include(a => a.Doctor)
            .ThenInclude(d => d.Specialization)
            .Include(a => a.Patient)
            .Where(a =>
                a.AppointmentDatetime < DateTime.UtcNow
                && a.Status != "Completed"
                && a.Status != "Cancelled"
            )
            .ToListAsync();

        return appointments.Select(AppointmentMapper.ToDomain);
    }

    public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime dateTime)
    {
        var conflictingAppointment = await _context.Appointments.FirstOrDefaultAsync(a =>
            a.DoctorId == doctorId && a.AppointmentDatetime == dateTime && a.Status != "Cancelled"
        );

        return conflictingAppointment == null;
    }

    public async Task<IEnumerable<DateTime>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        var bookedSlots = await _context
            .Appointments.Where(a =>
                a.DoctorId == doctorId
                && a.AppointmentDatetime >= startOfDay
                && a.AppointmentDatetime < endOfDay
                && a.Status != "Cancelled"
            )
            .Select(a => a.AppointmentDatetime)
            .ToListAsync();

        var availableSlots = new List<DateTime>();
        var currentTime = startOfDay.AddHours(8); // Start at 8 AM
        var endTime = startOfDay.AddHours(17); // End at 5 PM

        while (currentTime < endTime)
        {
            if (!bookedSlots.Contains(currentTime))
            {
                availableSlots.Add(currentTime);
            }
            currentTime = currentTime.AddMinutes(30); // 30-minute slots
        }

        return availableSlots;
    }

    public async Task<bool> CheckConflictAsync(
        int doctorId,
        DateTime dateTime,
        int? excludeAppointmentId = null
    )
    {
        var query = _context.Appointments.Where(a =>
            a.DoctorId == doctorId && a.AppointmentDatetime == dateTime && a.Status != "Cancelled"
        );

        if (excludeAppointmentId.HasValue)
        {
            query = query.Where(a => a.AppointmentId != excludeAppointmentId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Appointments.CountAsync();
    }

    public async Task<Dictionary<string, int>> GetStatusStatsAsync()
    {
        return await _context
            .Appointments.GroupBy(a => a.Status)
            .ToDictionaryAsync(g => g.Key ?? "Unknown", g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetDoctorStatsAsync()
    {
        return await _context
            .Appointments.Include(a => a.Doctor)
            .GroupBy(a => a.Doctor.FullName)
            .ToDictionaryAsync(g => g.Key ?? "Unknown", g => g.Count());
    }

    // Mapping moved to AppointmentMapper
}
