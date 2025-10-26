using AutoMapper;
using VisionCare.Application.DTOs.FollowUpDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.FollowUp;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.FollowUp;

public class FollowUpService : IFollowUpService
{
    private readonly IFollowUpRepository _followUpRepository;
    private readonly IMapper _mapper;

    public FollowUpService(IFollowUpRepository followUpRepository, IMapper mapper)
    {
        _followUpRepository = followUpRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FollowUpDto>> GetAllFollowUpsAsync()
    {
        var followUps = await _followUpRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FollowUpDto>>(followUps);
    }

    public async Task<FollowUpDto?> GetFollowUpByIdAsync(int id)
    {
        var followUp = await _followUpRepository.GetByIdAsync(id);
        return followUp != null ? _mapper.Map<FollowUpDto>(followUp) : null;
    }

    public async Task<FollowUpDto?> GetFollowUpByAppointmentIdAsync(int appointmentId)
    {
        var followUp = await _followUpRepository.GetByAppointmentIdAsync(appointmentId);
        return followUp != null ? _mapper.Map<FollowUpDto>(followUp) : null;
    }

    public async Task<IEnumerable<FollowUpDto>> GetFollowUpsByPatientIdAsync(int patientId)
    {
        var followUps = await _followUpRepository.GetByPatientIdAsync(patientId);
        return _mapper.Map<IEnumerable<FollowUpDto>>(followUps);
    }

    public async Task<IEnumerable<FollowUpDto>> GetFollowUpsByDoctorIdAsync(int doctorId)
    {
        var followUps = await _followUpRepository.GetByDoctorIdAsync(doctorId);
        return _mapper.Map<IEnumerable<FollowUpDto>>(followUps);
    }

    public async Task<(IEnumerable<FollowUpDto> items, int totalCount)> SearchFollowUpsAsync(FollowUpSearchRequest request)
    {
        var result = await _followUpRepository.SearchAsync(
            request.PatientId,
            request.DoctorId,
            request.Status,
            request.FromDate,
            request.ToDate,
            request.Page,
            request.PageSize
        );
        return (_mapper.Map<IEnumerable<FollowUpDto>>(result.items), result.totalCount);
    }

    public async Task<FollowUpDto> CreateFollowUpAsync(CreateFollowUpRequest request)
    {
        var followUp = _mapper.Map<Domain.Entities.FollowUp>(request);
        var createdFollowUp = await _followUpRepository.AddAsync(followUp);
        return _mapper.Map<FollowUpDto>(createdFollowUp);
    }

    public async Task<FollowUpDto> UpdateFollowUpAsync(int id, UpdateFollowUpRequest request)
    {
        var existingFollowUp = await _followUpRepository.GetByIdAsync(id);
        if (existingFollowUp == null)
        {
            throw new NotFoundException($"Follow-up with ID {id} not found.");
        }

        _mapper.Map(request, existingFollowUp);
        await _followUpRepository.UpdateAsync(existingFollowUp);
        return _mapper.Map<FollowUpDto>(existingFollowUp);
    }

    public async Task<FollowUpDto> CompleteFollowUpAsync(int id)
    {
        var existingFollowUp = await _followUpRepository.GetByIdAsync(id);
        if (existingFollowUp == null)
        {
            throw new NotFoundException($"Follow-up with ID {id} not found.");
        }

        existingFollowUp.Complete();
        await _followUpRepository.UpdateAsync(existingFollowUp);
        return _mapper.Map<FollowUpDto>(existingFollowUp);
    }

    public async Task<FollowUpDto> CancelFollowUpAsync(int id, string? reason = null)
    {
        var existingFollowUp = await _followUpRepository.GetByIdAsync(id);
        if (existingFollowUp == null)
        {
            throw new NotFoundException($"Follow-up with ID {id} not found.");
        }

        existingFollowUp.Cancel(reason);
        await _followUpRepository.UpdateAsync(existingFollowUp);
        return _mapper.Map<FollowUpDto>(existingFollowUp);
    }

    public async Task<FollowUpDto> RescheduleFollowUpAsync(int id, DateTime newDate)
    {
        var existingFollowUp = await _followUpRepository.GetByIdAsync(id);
        if (existingFollowUp == null)
        {
            throw new NotFoundException($"Follow-up with ID {id} not found.");
        }

        existingFollowUp.Reschedule(newDate);
        await _followUpRepository.UpdateAsync(existingFollowUp);
        return _mapper.Map<FollowUpDto>(existingFollowUp);
    }

    public async Task<bool> DeleteFollowUpAsync(int id)
    {
        var existingFollowUp = await _followUpRepository.GetByIdAsync(id);
        if (existingFollowUp == null)
        {
            return false;
        }

        await _followUpRepository.DeleteAsync(id);
        return true;
    }
}
