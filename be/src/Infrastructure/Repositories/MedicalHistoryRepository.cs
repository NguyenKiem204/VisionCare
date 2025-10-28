using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.MedicalHistory;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Models;

namespace VisionCare.Infrastructure.Repositories;

public class MedicalHistoryRepository : IMedicalHistoryRepository
{
    private readonly VisionCareDbContext _context;

    public MedicalHistoryRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MedicalHistory>> GetAllAsync()
    {
        var medicalHistories = await _context
            .Medicalhistories.Include(mh => mh.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(mh => mh.Appointment)
            .ThenInclude(a => a.Patient)
            .ToListAsync();

        return medicalHistories.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<MedicalHistory?> GetByIdAsync(int id)
    {
        var medicalHistory = await _context
            .Medicalhistories.Include(mh => mh.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(mh => mh.Appointment)
            .ThenInclude(a => a.Patient)
            .FirstOrDefaultAsync(mh => mh.MedicalHistoryId == id);

        return medicalHistory != null ? ConvertToDomainEntity(medicalHistory) : null;
    }

    public async Task<MedicalHistory?> GetByAppointmentIdAsync(int appointmentId)
    {
        var medicalHistory = await _context
            .Medicalhistories.Include(mh => mh.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(mh => mh.Appointment)
            .ThenInclude(a => a.Patient)
            .FirstOrDefaultAsync(mh => mh.AppointmentId == appointmentId);

        return medicalHistory != null ? ConvertToDomainEntity(medicalHistory) : null;
    }

    public async Task<IEnumerable<MedicalHistory>> GetByPatientIdAsync(int patientId)
    {
        var medicalHistories = await _context
            .Medicalhistories.Include(mh => mh.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(mh => mh.Appointment)
            .ThenInclude(a => a.Patient)
            .Where(mh => mh.Appointment.PatientId == patientId)
            .ToListAsync();

        return medicalHistories.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<MedicalHistory>> GetByDoctorIdAsync(int doctorId)
    {
        var medicalHistories = await _context
            .Medicalhistories.Include(mh => mh.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(mh => mh.Appointment)
            .ThenInclude(a => a.Patient)
            .Where(mh => mh.Appointment.DoctorId == doctorId)
            .ToListAsync();

        return medicalHistories.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<(IEnumerable<MedicalHistory> items, int totalCount)> SearchAsync(
        string? keyword,
        int? patientId,
        int? doctorId,
        DateTime? fromDate,
        DateTime? toDate,
        string? diagnosis,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null,
        bool desc = false
    )
    {
        var query = _context
            .Medicalhistories.Include(mh => mh.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(mh => mh.Appointment)
            .ThenInclude(a => a.Patient)
            .AsQueryable();

        if (patientId.HasValue)
        {
            query = query.Where(mh => mh.Appointment.PatientId == patientId);
        }

        if (doctorId.HasValue)
        {
            query = query.Where(mh => mh.Appointment.DoctorId == doctorId);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(mh => mh.Appointment.AppointmentDatetime >= fromDate);
        }

        if (toDate.HasValue)
        {
            query = query.Where(mh => mh.Appointment.AppointmentDatetime <= toDate);
        }

        if (!string.IsNullOrEmpty(diagnosis))
        {
            query = query.Where(mh => mh.Diagnosis != null && mh.Diagnosis.Contains(diagnosis));
        }

        if (!string.IsNullOrEmpty(keyword))
        {
            var loweredKeyword = keyword.Trim().ToLower();
            query = query.Where(mh =>
                (mh.Diagnosis != null && mh.Diagnosis.ToLower().Contains(loweredKeyword)) ||
                (mh.Symptoms != null && mh.Symptoms.ToLower().Contains(loweredKeyword)) ||
                (mh.Treatment != null && mh.Treatment.ToLower().Contains(loweredKeyword)) ||
                (mh.Appointment.Patient != null && mh.Appointment.Patient.FullName.ToLower().Contains(loweredKeyword)) ||
                (mh.Appointment.Doctor != null && mh.Appointment.Doctor.FullName.ToLower().Contains(loweredKeyword))
            );
        }

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "patientname" => desc ? query.OrderByDescending(mh => mh.Appointment.Patient.FullName) : query.OrderBy(mh => mh.Appointment.Patient.FullName),
            "doctorname" => desc ? query.OrderByDescending(mh => mh.Appointment.Doctor.FullName) : query.OrderBy(mh => mh.Appointment.Doctor.FullName),
            "appointmentdate" => desc ? query.OrderByDescending(mh => mh.Appointment.AppointmentDatetime) : query.OrderBy(mh => mh.Appointment.AppointmentDatetime),
            "diagnosis" => desc ? query.OrderByDescending(mh => mh.Diagnosis) : query.OrderBy(mh => mh.Diagnosis),
            _ => desc ? query.OrderByDescending(mh => mh.MedicalHistoryId) : query.OrderBy(mh => mh.MedicalHistoryId)
        };

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items.Select(ConvertToDomainEntity), totalCount);
    }

    public async Task<MedicalHistory> AddAsync(MedicalHistory medicalHistory)
    {
        var infrastructureModel = ConvertToInfrastructureModel(medicalHistory);
        _context.Medicalhistories.Add(infrastructureModel);
        await _context.SaveChangesAsync();

        // Return the domain entity with updated ID
        medicalHistory.Id = infrastructureModel.MedicalHistoryId;
        return medicalHistory;
    }

    public async Task UpdateAsync(MedicalHistory medicalHistory)
    {
        var existingModel = await _context.Medicalhistories.FindAsync(medicalHistory.Id);
        if (existingModel != null)
        {
            UpdateInfrastructureModel(existingModel, medicalHistory);
            _context.Medicalhistories.Update(existingModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var medicalHistory = await _context.Medicalhistories.FindAsync(id);
        if (medicalHistory != null)
        {
            _context.Medicalhistories.Remove(medicalHistory);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Medicalhistories.AnyAsync(mh => mh.MedicalHistoryId == id);
    }

    public async Task<bool> ExistsForAppointmentAsync(int appointmentId)
    {
        return await _context.Medicalhistories.AnyAsync(mh => mh.AppointmentId == appointmentId);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Medicalhistories.CountAsync();
    }

    public async Task<IEnumerable<MedicalHistory>> GetPagedAsync(int page, int pageSize)
    {
        var medicalHistories = await _context
            .Medicalhistories.Include(mh => mh.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(mh => mh.Appointment)
            .ThenInclude(a => a.Patient)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return medicalHistories.Select(ConvertToDomainEntity).ToList();
    }

    private static MedicalHistory ConvertToDomainEntity(Infrastructure.Models.Medicalhistory model)
    {
        return new MedicalHistory
        {
            Id = model.MedicalHistoryId,
            AppointmentId = model.AppointmentId,
            Diagnosis = model.Diagnosis,
            Symptoms = model.Symptoms,
            Treatment = model.Treatment,
            Prescription = model.Prescription,
            VisionLeft = model.VisionLeft,
            VisionRight = model.VisionRight,
            AdditionalTests = model.AdditionalTests,
            Notes = model.Notes,
            Created = model.CreatedAt ?? DateTime.UtcNow,
            LastModified = model.UpdatedAt,
            Appointment = model.Appointment != null ? new Domain.Entities.Appointment
            {
                Id = model.Appointment.AppointmentId,
                AppointmentDate = model.Appointment.AppointmentDatetime,
                Doctor = model.Appointment.Doctor != null ? new Domain.Entities.Doctor
                {
                    Id = model.Appointment.Doctor.AccountId,
                    DoctorName = model.Appointment.Doctor.FullName
                } : null,
                Patient = model.Appointment.Patient != null ? new Domain.Entities.Customer
                {
                    Id = model.Appointment.Patient.AccountId,
                    CustomerName = model.Appointment.Patient.FullName
                } : null
            } : null
        };
    }

    private static Infrastructure.Models.Medicalhistory ConvertToInfrastructureModel(
        MedicalHistory domainEntity
    )
    {
        return new Infrastructure.Models.Medicalhistory
        {
            MedicalHistoryId = domainEntity.Id,
            AppointmentId = domainEntity.AppointmentId,
            Diagnosis = domainEntity.Diagnosis,
            Symptoms = domainEntity.Symptoms,
            Treatment = domainEntity.Treatment,
            Prescription = domainEntity.Prescription,
            VisionLeft = domainEntity.VisionLeft,
            VisionRight = domainEntity.VisionRight,
            AdditionalTests = domainEntity.AdditionalTests,
            Notes = domainEntity.Notes,
            CreatedAt = domainEntity.Created,
            UpdatedAt = domainEntity.LastModified,
        };
    }

    private static void UpdateInfrastructureModel(
        Infrastructure.Models.Medicalhistory model,
        MedicalHistory domainEntity
    )
    {
        model.Diagnosis = domainEntity.Diagnosis;
        model.Symptoms = domainEntity.Symptoms;
        model.Treatment = domainEntity.Treatment;
        model.Prescription = domainEntity.Prescription;
        model.VisionLeft = domainEntity.VisionLeft;
        model.VisionRight = domainEntity.VisionRight;
        model.AdditionalTests = domainEntity.AdditionalTests;
        model.Notes = domainEntity.Notes;
        model.UpdatedAt = domainEntity.LastModified;
    }
}
