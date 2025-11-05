using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.FollowUp;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;
using InfrastructureFollowUp = VisionCare.Infrastructure.Models.Followup;

namespace VisionCare.Infrastructure.Repositories;

public class FollowUpRepository : IFollowUpRepository
{
    private readonly VisionCareDbContext _context;

    public FollowUpRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Domain.Entities.FollowUp>> GetAllAsync()
    {
        var followUps = await _context
            .Followups.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .ToListAsync();

        return followUps.Select(ConvertToDomainEntity);
    }

    public async Task<Domain.Entities.FollowUp?> GetByIdAsync(int id)
    {
        var followUp = await _context
            .Followups.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .FirstOrDefaultAsync(f => f.FollowUpId == id);

        return followUp != null ? ConvertToDomainEntity(followUp) : null;
    }

    public async Task<Domain.Entities.FollowUp?> GetByAppointmentIdAsync(int appointmentId)
    {
        var followUp = await _context
            .Followups.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .FirstOrDefaultAsync(f => f.AppointmentId == appointmentId);

        return followUp != null ? ConvertToDomainEntity(followUp) : null;
    }

    public async Task<IEnumerable<Domain.Entities.FollowUp>> GetByPatientIdAsync(int patientId)
    {
        var followUps = await _context
            .Followups.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Where(f => f.Appointment.PatientId == patientId)
            .ToListAsync();

        return followUps.Select(ConvertToDomainEntity);
    }

    public async Task<IEnumerable<Domain.Entities.FollowUp>> GetByDoctorIdAsync(int doctorId)
    {
        var followUps = await _context
            .Followups.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Where(f => f.Appointment.DoctorId == doctorId)
            .ToListAsync();

        return followUps.Select(ConvertToDomainEntity);
    }

    public async Task<Domain.Entities.FollowUp> AddAsync(Domain.Entities.FollowUp followUp)
    {
        var followUpModel = ConvertToInfrastructureModel(followUp);
        _context.Followups.Add(followUpModel);
        await _context.SaveChangesAsync();
        return ConvertToDomainEntity(followUpModel);
    }

    public async Task UpdateAsync(Domain.Entities.FollowUp followUp)
    {
        var existingFollowUp = await _context.Followups.FirstOrDefaultAsync(f =>
            f.FollowUpId == followUp.Id
        );
        if (existingFollowUp != null)
        {
            existingFollowUp.NextAppointmentDate = followUp.NextAppointmentDate.HasValue
                ? DateOnly.FromDateTime(followUp.NextAppointmentDate.Value)
                : null;
            existingFollowUp.Description = followUp.Description;
            existingFollowUp.Status = followUp.Status;
            existingFollowUp.CreatedAt = followUp.Created;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var followUp = await _context.Followups.FindAsync(id);
        if (followUp != null)
        {
            _context.Followups.Remove(followUp);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(IEnumerable<Domain.Entities.FollowUp> items, int totalCount)> SearchAsync(
        int? patientId,
        int? doctorId,
        string? status,
        DateTime? fromDate,
        DateTime? toDate,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var query = _context
            .Followups.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .AsQueryable();

        if (patientId.HasValue)
        {
            query = query.Where(f => f.Appointment.PatientId == patientId.Value);
        }

        if (doctorId.HasValue)
        {
            query = query.Where(f => f.Appointment.DoctorId == doctorId.Value);
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(f => f.Status == status);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(f =>
                f.NextAppointmentDate >= DateOnly.FromDateTime(fromDate.Value)
            );
        }

        if (toDate.HasValue)
        {
            query = query.Where(f => f.NextAppointmentDate <= DateOnly.FromDateTime(toDate.Value));
        }

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "patientname" => desc
                ? query.OrderByDescending(f => f.Appointment.Patient.FullName)
                : query.OrderBy(f => f.Appointment.Patient.FullName),
            "doctorname" => desc
                ? query.OrderByDescending(f => f.Appointment.Doctor.FullName)
                : query.OrderBy(f => f.Appointment.Doctor.FullName),
            "nextappointmentdate" => desc
                ? query.OrderByDescending(f => f.NextAppointmentDate)
                : query.OrderBy(f => f.NextAppointmentDate),
            "status" => desc
                ? query.OrderByDescending(f => f.Status)
                : query.OrderBy(f => f.Status),
            _ => desc
                ? query.OrderByDescending(f => f.FollowUpId)
                : query.OrderBy(f => f.FollowUpId),
        };

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items.Select(ConvertToDomainEntity), totalCount);
    }

    private static Domain.Entities.FollowUp ConvertToDomainEntity(InfrastructureFollowUp model)
    {
        return new Domain.Entities.FollowUp
        {
            Id = model.FollowUpId,
            AppointmentId = model.AppointmentId,
            NextAppointmentDate = model.NextAppointmentDate?.ToDateTime(TimeOnly.MinValue),
            Description = model.Description ?? string.Empty,
            Status = model.Status ?? "Pending",
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = model.CreatedAt ?? DateTime.UtcNow,
            Appointment =
                model.Appointment != null
                    ? new Appointment
                    {
                        Id = model.Appointment.AppointmentId,
                        PatientId = model.Appointment.PatientId,
                        DoctorId = model.Appointment.DoctorId,
                        AppointmentDate = model.Appointment.AppointmentDatetime,
                        AppointmentStatus = model.Appointment.Status ?? "Pending",
                        Patient =
                            model.Appointment.Patient != null
                                ? new Customer
                                {
                                    Id = model.Appointment.Patient.AccountId,
                                    CustomerName =
                                        model.Appointment.Patient.FullName ?? string.Empty,
                                }
                                : null!,
                        Doctor =
                            model.Appointment.Doctor != null
                                ? new Doctor
                                {
                                    Id = model.Appointment.Doctor.AccountId,
                                    DoctorName = model.Appointment.Doctor.FullName ?? string.Empty,
                                }
                                : null!,
                    }
                    : null!,
        };
    }

    private static InfrastructureFollowUp ConvertToInfrastructureModel(
        Domain.Entities.FollowUp domain
    )
    {
        return new InfrastructureFollowUp
        {
            FollowUpId = domain.Id,
            AppointmentId = domain.AppointmentId,
            NextAppointmentDate = domain.NextAppointmentDate.HasValue
                ? DateOnly.FromDateTime(domain.NextAppointmentDate.Value)
                : null,
            Description = domain.Description,
            Status = domain.Status,
            CreatedAt = domain.Created,
        };
    }
}
