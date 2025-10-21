using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces.Feedback;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Models;

namespace VisionCare.Infrastructure.Repositories;

public class FeedbackServiceRepository : IFeedbackServiceRepository
{
    private readonly VisionCareDbContext _context;

    public FeedbackServiceRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FeedbackService>> GetAllAsync()
    {
        var feedbacks = await _context
            .Feedbackservices.Include(f => f.Appointment)
            .ThenInclude(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<FeedbackService?> GetByIdAsync(int id)
    {
        var feedback = await _context
            .Feedbackservices.Include(f => f.Appointment)
            .ThenInclude(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .FirstOrDefaultAsync(f => f.FeedbackId == id);

        return feedback != null ? ConvertToDomainEntity(feedback) : null;
    }

    public async Task<FeedbackService?> GetByAppointmentIdAsync(int appointmentId)
    {
        var feedback = await _context
            .Feedbackservices.Include(f => f.Appointment)
            .ThenInclude(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .FirstOrDefaultAsync(f => f.AppointmentId == appointmentId);

        return feedback != null ? ConvertToDomainEntity(feedback) : null;
    }

    public async Task<IEnumerable<FeedbackService>> GetByServiceIdAsync(int serviceId)
    {
        var feedbacks = await _context
            .Feedbackservices.Include(f => f.Appointment)
            .ThenInclude(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Where(f => f.Appointment.ServiceDetail.ServiceId == serviceId)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<FeedbackService>> GetByPatientIdAsync(int patientId)
    {
        var feedbacks = await _context
            .Feedbackservices.Include(f => f.Appointment)
            .ThenInclude(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Where(f => f.Appointment.PatientId == patientId)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<FeedbackService>> GetByRatingRangeAsync(
        int minRating,
        int maxRating
    )
    {
        var feedbacks = await _context
            .Feedbackservices.Include(f => f.Appointment)
            .ThenInclude(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Where(f => f.Rating >= minRating && f.Rating <= maxRating)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<IEnumerable<FeedbackService>> GetUnrespondedAsync()
    {
        var feedbacks = await _context
            .Feedbackservices.Include(f => f.Appointment)
            .ThenInclude(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Where(f => f.ResponseText == null)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    public async Task<FeedbackService> AddAsync(FeedbackService feedback)
    {
        var infrastructureModel = ConvertToInfrastructureModel(feedback);
        _context.Feedbackservices.Add(infrastructureModel);
        await _context.SaveChangesAsync();

        // Return the domain entity with updated ID
        feedback.Id = infrastructureModel.FeedbackId;
        return feedback;
    }

    public async Task UpdateAsync(FeedbackService feedback)
    {
        var existingModel = await _context.Feedbackservices.FindAsync(feedback.Id);
        if (existingModel != null)
        {
            UpdateInfrastructureModel(existingModel, feedback);
            _context.Feedbackservices.Update(existingModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var feedback = await _context.Feedbackservices.FindAsync(id);
        if (feedback != null)
        {
            _context.Feedbackservices.Remove(feedback);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Feedbackservices.AnyAsync(f => f.FeedbackId == id);
    }

    public async Task<bool> ExistsForAppointmentAsync(int appointmentId)
    {
        return await _context.Feedbackservices.AnyAsync(f => f.AppointmentId == appointmentId);
    }

    public async Task<double> GetAverageRatingByServiceIdAsync(int serviceId)
    {
        var average = await _context
            .Feedbackservices.Where(f => f.Appointment.ServiceDetail.ServiceId == serviceId)
            .AverageAsync(f => (double?)f.Rating);

        return average ?? 0.0;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Feedbackservices.CountAsync();
    }

    public async Task<IEnumerable<FeedbackService>> GetPagedAsync(int page, int pageSize)
    {
        var feedbacks = await _context
            .Feedbackservices.Include(f => f.Appointment)
            .ThenInclude(a => a.ServiceDetail)
            .ThenInclude(sd => sd.Service)
            .Include(f => f.Appointment)
            .ThenInclude(a => a.Patient)
            .Include(f => f.RespondedByNavigation)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return feedbacks.Select(ConvertToDomainEntity).ToList();
    }

    private static FeedbackService ConvertToDomainEntity(
        Infrastructure.Models.Feedbackservice model
    )
    {
        return new FeedbackService
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

    private static Infrastructure.Models.Feedbackservice ConvertToInfrastructureModel(
        FeedbackService domainEntity
    )
    {
        return new Infrastructure.Models.Feedbackservice
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
        Infrastructure.Models.Feedbackservice model,
        FeedbackService domainEntity
    )
    {
        model.Rating = domainEntity.Rating > 0 ? (short?)(short)domainEntity.Rating : null;
        model.FeedbackText = domainEntity.FeedbackText;
        model.RespondedBy = domainEntity.RespondedBy;
        model.ResponseText = domainEntity.ResponseText;
        model.ResponseDate = domainEntity.ResponseDate;
    }
}
