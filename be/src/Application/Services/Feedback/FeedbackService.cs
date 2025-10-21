using AutoMapper;
using VisionCare.Application.DTOs.FeedbackDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.Feedback;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Feedback;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackDoctorRepository _doctorFeedbackRepository;
    private readonly IFeedbackServiceRepository _serviceFeedbackRepository;
    private readonly IMapper _mapper;

    public FeedbackService(
        IFeedbackDoctorRepository doctorFeedbackRepository,
        IFeedbackServiceRepository serviceFeedbackRepository,
        IMapper mapper
    )
    {
        _doctorFeedbackRepository = doctorFeedbackRepository;
        _serviceFeedbackRepository = serviceFeedbackRepository;
        _mapper = mapper;
    }

    #region Doctor Feedback Management

    public async Task<IEnumerable<FeedbackDoctorDto>> GetAllDoctorFeedbacksAsync()
    {
        var feedbacks = await _doctorFeedbackRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FeedbackDoctorDto>>(feedbacks);
    }

    public async Task<FeedbackDoctorDto?> GetDoctorFeedbackByIdAsync(int id)
    {
        var feedback = await _doctorFeedbackRepository.GetByIdAsync(id);
        return feedback != null ? _mapper.Map<FeedbackDoctorDto>(feedback) : null;
    }

    public async Task<FeedbackDoctorDto?> GetDoctorFeedbackByAppointmentIdAsync(int appointmentId)
    {
        var feedback = await _doctorFeedbackRepository.GetByAppointmentIdAsync(appointmentId);
        return feedback != null ? _mapper.Map<FeedbackDoctorDto>(feedback) : null;
    }

    public async Task<IEnumerable<FeedbackDoctorDto>> GetDoctorFeedbacksByDoctorIdAsync(
        int doctorId
    )
    {
        var feedbacks = await _doctorFeedbackRepository.GetByDoctorIdAsync(doctorId);
        return _mapper.Map<IEnumerable<FeedbackDoctorDto>>(feedbacks);
    }

    public async Task<IEnumerable<FeedbackDoctorDto>> GetDoctorFeedbacksByPatientIdAsync(
        int patientId
    )
    {
        var feedbacks = await _doctorFeedbackRepository.GetByPatientIdAsync(patientId);
        return _mapper.Map<IEnumerable<FeedbackDoctorDto>>(feedbacks);
    }

    public async Task<FeedbackDoctorDto> CreateDoctorFeedbackAsync(
        CreateFeedbackDoctorRequest request
    )
    {
        // Check if feedback already exists for this appointment
        if (await _doctorFeedbackRepository.ExistsForAppointmentAsync(request.AppointmentId))
        {
            throw new ValidationException("Feedback already exists for this appointment.");
        }

        // Use AutoMapper to create entity from DTO
        var feedback = _mapper.Map<FeedbackDoctor>(request);
        feedback.FeedbackDate = DateTime.UtcNow;
        feedback.Created = DateTime.UtcNow;

        var createdFeedback = await _doctorFeedbackRepository.AddAsync(feedback);
        return _mapper.Map<FeedbackDoctorDto>(createdFeedback);
    }

    public async Task<FeedbackDoctorDto> UpdateDoctorFeedbackAsync(
        int id,
        UpdateFeedbackDoctorRequest request
    )
    {
        var existingFeedback = await _doctorFeedbackRepository.GetByIdAsync(id);
        if (existingFeedback == null)
        {
            throw new NotFoundException($"Doctor feedback with ID {id} not found.");
        }

        // Use domain methods to update
        existingFeedback.UpdateRating(request.Rating);
        existingFeedback.UpdateFeedback(request.FeedbackText);

        await _doctorFeedbackRepository.UpdateAsync(existingFeedback);
        return _mapper.Map<FeedbackDoctorDto>(existingFeedback);
    }

    public async Task<FeedbackDoctorDto> RespondToDoctorFeedbackAsync(
        int id,
        RespondToFeedbackRequest request
    )
    {
        var existingFeedback = await _doctorFeedbackRepository.GetByIdAsync(id);
        if (existingFeedback == null)
        {
            throw new NotFoundException($"Doctor feedback with ID {id} not found.");
        }

        // Use domain method to add response
        existingFeedback.AddResponse(1, request.ResponseText); // TODO: Get actual user ID from context

        await _doctorFeedbackRepository.UpdateAsync(existingFeedback);
        return _mapper.Map<FeedbackDoctorDto>(existingFeedback);
    }

    public async Task<bool> DeleteDoctorFeedbackAsync(int id)
    {
        var existingFeedback = await _doctorFeedbackRepository.GetByIdAsync(id);
        if (existingFeedback == null)
        {
            return false;
        }

        await _doctorFeedbackRepository.DeleteAsync(id);
        return true;
    }

    #endregion

    #region Service Feedback Management

    public async Task<IEnumerable<FeedbackServiceDto>> GetAllServiceFeedbacksAsync()
    {
        var feedbacks = await _serviceFeedbackRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FeedbackServiceDto>>(feedbacks);
    }

    public async Task<FeedbackServiceDto?> GetServiceFeedbackByIdAsync(int id)
    {
        var feedback = await _serviceFeedbackRepository.GetByIdAsync(id);
        return feedback != null ? _mapper.Map<FeedbackServiceDto>(feedback) : null;
    }

    public async Task<FeedbackServiceDto?> GetServiceFeedbackByAppointmentIdAsync(int appointmentId)
    {
        var feedback = await _serviceFeedbackRepository.GetByAppointmentIdAsync(appointmentId);
        return feedback != null ? _mapper.Map<FeedbackServiceDto>(feedback) : null;
    }

    public async Task<IEnumerable<FeedbackServiceDto>> GetServiceFeedbacksByServiceIdAsync(
        int serviceId
    )
    {
        var feedbacks = await _serviceFeedbackRepository.GetByServiceIdAsync(serviceId);
        return _mapper.Map<IEnumerable<FeedbackServiceDto>>(feedbacks);
    }

    public async Task<IEnumerable<FeedbackServiceDto>> GetServiceFeedbacksByPatientIdAsync(
        int patientId
    )
    {
        var feedbacks = await _serviceFeedbackRepository.GetByPatientIdAsync(patientId);
        return _mapper.Map<IEnumerable<FeedbackServiceDto>>(feedbacks);
    }

    public async Task<FeedbackServiceDto> CreateServiceFeedbackAsync(
        CreateFeedbackServiceRequest request
    )
    {
        // Check if feedback already exists for this appointment
        if (await _serviceFeedbackRepository.ExistsForAppointmentAsync(request.AppointmentId))
        {
            throw new ValidationException("Feedback already exists for this appointment.");
        }

        // Use AutoMapper to create entity from DTO
        var feedback = _mapper.Map<VisionCare.Domain.Entities.FeedbackService>(request);
        feedback.FeedbackDate = DateTime.UtcNow;
        feedback.Created = DateTime.UtcNow;

        var createdFeedback = await _serviceFeedbackRepository.AddAsync(feedback);
        return _mapper.Map<FeedbackServiceDto>(createdFeedback);
    }

    public async Task<FeedbackServiceDto> UpdateServiceFeedbackAsync(
        int id,
        UpdateFeedbackServiceRequest request
    )
    {
        var existingFeedback = await _serviceFeedbackRepository.GetByIdAsync(id);
        if (existingFeedback == null)
        {
            throw new NotFoundException($"Service feedback with ID {id} not found.");
        }

        // Use domain methods to update
        existingFeedback.UpdateRating(request.Rating);
        existingFeedback.UpdateFeedback(request.FeedbackText);

        await _serviceFeedbackRepository.UpdateAsync(existingFeedback);
        return _mapper.Map<FeedbackServiceDto>(existingFeedback);
    }

    public async Task<FeedbackServiceDto> RespondToServiceFeedbackAsync(
        int id,
        RespondToServiceFeedbackRequest request
    )
    {
        var existingFeedback = await _serviceFeedbackRepository.GetByIdAsync(id);
        if (existingFeedback == null)
        {
            throw new NotFoundException($"Service feedback with ID {id} not found.");
        }

        // Use domain method to add response
        existingFeedback.AddResponse(1, request.ResponseText); // TODO: Get actual user ID from context

        await _serviceFeedbackRepository.UpdateAsync(existingFeedback);
        return _mapper.Map<FeedbackServiceDto>(existingFeedback);
    }

    public async Task<bool> DeleteServiceFeedbackAsync(int id)
    {
        var existingFeedback = await _serviceFeedbackRepository.GetByIdAsync(id);
        if (existingFeedback == null)
        {
            return false;
        }

        await _serviceFeedbackRepository.DeleteAsync(id);
        return true;
    }

    #endregion

    #region Search and Analytics

    public async Task<IEnumerable<FeedbackDoctorDto>> SearchDoctorFeedbacksAsync(
        FeedbackSearchRequest request
    )
    {
        var feedbacks = await _doctorFeedbackRepository.GetByDoctorIdAsync(request.DoctorId ?? 0);

        // Apply additional filters
        if (request.PatientId.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.Appointment.PatientId == request.PatientId.Value);
        }

        if (request.MinRating.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.Rating >= request.MinRating.Value);
        }

        if (request.MaxRating.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.Rating <= request.MaxRating.Value);
        }

        if (request.FromDate.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.FeedbackDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.FeedbackDate <= request.ToDate.Value);
        }

        if (request.HasResponse.HasValue)
        {
            if (request.HasResponse.Value)
            {
                feedbacks = feedbacks.Where(f => f.HasResponse());
            }
            else
            {
                feedbacks = feedbacks.Where(f => !f.HasResponse());
            }
        }

        return _mapper.Map<IEnumerable<FeedbackDoctorDto>>(feedbacks);
    }

    public async Task<IEnumerable<FeedbackServiceDto>> SearchServiceFeedbacksAsync(
        ServiceFeedbackSearchRequest request
    )
    {
        var feedbacks = await _serviceFeedbackRepository.GetByServiceIdAsync(
            request.ServiceId ?? 0
        );

        // Apply additional filters
        if (request.PatientId.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.Appointment.PatientId == request.PatientId.Value);
        }

        if (request.MinRating.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.Rating >= request.MinRating.Value);
        }

        if (request.MaxRating.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.Rating <= request.MaxRating.Value);
        }

        if (request.FromDate.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.FeedbackDate >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            feedbacks = feedbacks.Where(f => f.FeedbackDate <= request.ToDate.Value);
        }

        if (request.HasResponse.HasValue)
        {
            if (request.HasResponse.Value)
            {
                feedbacks = feedbacks.Where(f => f.HasResponse());
            }
            else
            {
                feedbacks = feedbacks.Where(f => !f.HasResponse());
            }
        }

        return _mapper.Map<IEnumerable<FeedbackServiceDto>>(feedbacks);
    }

    public async Task<double> GetDoctorAverageRatingAsync(int doctorId)
    {
        return await _doctorFeedbackRepository.GetAverageRatingByDoctorIdAsync(doctorId);
    }

    public async Task<double> GetServiceAverageRatingAsync(int serviceId)
    {
        return await _serviceFeedbackRepository.GetAverageRatingByServiceIdAsync(serviceId);
    }

    public async Task<IEnumerable<FeedbackDoctorDto>> GetUnrespondedDoctorFeedbacksAsync()
    {
        var feedbacks = await _doctorFeedbackRepository.GetUnrespondedAsync();
        return _mapper.Map<IEnumerable<FeedbackDoctorDto>>(feedbacks);
    }

    public async Task<IEnumerable<FeedbackServiceDto>> GetUnrespondedServiceFeedbacksAsync()
    {
        var feedbacks = await _serviceFeedbackRepository.GetUnrespondedAsync();
        return _mapper.Map<IEnumerable<FeedbackServiceDto>>(feedbacks);
    }

    public async Task<int> GetTotalDoctorFeedbacksCountAsync()
    {
        return await _doctorFeedbackRepository.GetTotalCountAsync();
    }

    public async Task<int> GetTotalServiceFeedbacksCountAsync()
    {
        return await _serviceFeedbackRepository.GetTotalCountAsync();
    }

    #endregion
}
