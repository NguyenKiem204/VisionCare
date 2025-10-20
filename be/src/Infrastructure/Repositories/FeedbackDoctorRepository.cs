using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Feedback;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Models;

namespace VisionCare.Infrastructure.Repositories;

public class FeedbackDoctorRepository : IFeedbackDoctorRepository
{
    private readonly VisionCareDbContext _context;

    public FeedbackDoctorRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FeedbackDoctor>> GetAllAsync()
    {
        var feedbacks = await _context
            .Feedbackdoctors.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<FeedbackDoctor?> GetByIdAsync(int id)
    {
        var feedback = await _context
            .Feedbackdoctors.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .FirstOrDefaultAsync(f => f.FeedbackId == id);

        return feedback != null ? ConvertToDomainEntity(feedback) : null;
    }

    public async Task<FeedbackDoctor?> GetByAppointmentIdAsync(int appointmentId)
    {
        var feedback = await _context
            .Feedbackdoctors.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .FirstOrDefaultAsync(f => f.AppointmentId == appointmentId);

        return feedback != null ? ConvertToDomainEntity(feedback) : null;
    }

    public async Task<IEnumerable<FeedbackDoctor>> GetByDoctorIdAsync(int doctorId)
    {
        var feedbacks = await _context
            .Feedbackdoctors.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Where(f => f.Appointment.DoctorId == doctorId)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<FeedbackDoctor>> GetByPatientIdAsync(int patientId)
    {
        var feedbacks = await _context
            .Feedbackdoctors.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Where(f => f.Appointment.PatientId == patientId)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<FeedbackDoctor>> GetByRatingRangeAsync(
        int minRating,
        int maxRating
    )
    {
        var feedbacks = await _context
            .Feedbackdoctors.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Where(f => f.Rating >= minRating && f.Rating <= maxRating)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<FeedbackDoctor>> GetUnrespondedAsync()
    {
        var feedbacks = await _context
            .Feedbackdoctors.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Where(f => f.ResponseText == null)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<FeedbackDoctor> AddAsync(FeedbackDoctor feedback)
    {
        var infrastructureModel = ConvertToInfrastructureModel(feedback);
        _context.Feedbackdoctors.Add(infrastructureModel);
        await _context.SaveChangesAsync();

        // Return the domain entity with updated ID
        feedback.Id = infrastructureModel.FeedbackId;
        return feedback;
    }

    public async Task UpdateAsync(FeedbackDoctor feedback)
    {
        var existingModel = await _context.Feedbackdoctors.FindAsync(feedback.Id);
        if (existingModel != null)
        {
            UpdateInfrastructureModel(existingModel, feedback);
            _context.Feedbackdoctors.Update(existingModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var feedback = await _context.Feedbackdoctors.FindAsync(id);
        if (feedback != null)
        {
            _context.Feedbackdoctors.Remove(feedback);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Feedbackdoctors.AnyAsync(f => f.FeedbackId == id);
    }

    public async Task<bool> ExistsForAppointmentAsync(int appointmentId)
    {
        return await _context.Feedbackdoctors.AnyAsync(f => f.AppointmentId == appointmentId);
    }

    public async Task<double> GetAverageRatingByDoctorIdAsync(int doctorId)
    {
        var average = await _context
            .Feedbackdoctors.Where(f => f.Appointment.DoctorId == doctorId)
            .AverageAsync(f => (double?)f.Rating);

        return average ?? 0.0;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Feedbackdoctors.CountAsync();
    }

    public async Task<IEnumerable<FeedbackDoctor>> GetPagedAsync(int page, int pageSize)
    {
        var feedbacks = await _context
            .Feedbackdoctors.Include(f => f.Appointment)
            .ThenInclude(a => a.Doctor)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    private static FeedbackDoctor ConvertToDomainEntity(Infrastructure.Models.Feedbackdoctor model)
    {
        return new FeedbackDoctor
        {
            Id = model.FeedbackId,
            AppointmentId = model.AppointmentId,
            Rating = model.Rating.HasValue ? (int)model.Rating.Value : 0,
            FeedbackText = model.FeedbackText,
            FeedbackDate = model.FeedbackDate ?? DateTime.UtcNow,
            RespondedBy = model.RespondedBy,
            ResponseText = model.ResponseText,
            ResponseDate = model.ResponseDate,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };
    }

    private static Infrastructure.Models.Feedbackdoctor ConvertToInfrastructureModel(
        FeedbackDoctor domainEntity
    )
    {
        return new Infrastructure.Models.Feedbackdoctor
        {
            FeedbackId = domainEntity.Id,
            AppointmentId = domainEntity.AppointmentId,
            Rating = domainEntity.Rating > 0 ? (short?)(short)domainEntity.Rating : null,
            FeedbackText = domainEntity.FeedbackText,
            FeedbackDate = domainEntity.FeedbackDate,
            RespondedBy = domainEntity.RespondedBy,
            ResponseText = domainEntity.ResponseText,
            ResponseDate = domainEntity.ResponseDate,
        };
    }

    private static void UpdateInfrastructureModel(
        Infrastructure.Models.Feedbackdoctor model,
        FeedbackDoctor domainEntity
    )
    {
        model.Rating = domainEntity.Rating > 0 ? (short?)(short)domainEntity.Rating : null;
        model.FeedbackText = domainEntity.FeedbackText;
        model.RespondedBy = domainEntity.RespondedBy;
        model.ResponseText = domainEntity.ResponseText;
        model.ResponseDate = domainEntity.ResponseDate;
    }
}
