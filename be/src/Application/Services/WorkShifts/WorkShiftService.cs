using AutoMapper;
using VisionCare.Application.DTOs.WorkShiftDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.WorkShifts;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.WorkShifts;

public class WorkShiftService : IWorkShiftService
{
    private readonly IWorkShiftRepository _workShiftRepository;
    private readonly IMapper _mapper;

    public WorkShiftService(IWorkShiftRepository workShiftRepository, IMapper mapper)
    {
        _workShiftRepository = workShiftRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WorkShiftDto>> GetAllWorkShiftsAsync()
    {
        var shifts = await _workShiftRepository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<WorkShiftDto>>(shifts);

        // Calculate duration minutes
        foreach (var dto in dtos)
        {
            var shift = shifts.First(s => s.Id == dto.Id);
            dto.DurationMinutes = shift.GetDurationMinutes();
        }

        return dtos;
    }

    public async Task<WorkShiftDto?> GetWorkShiftByIdAsync(int id)
    {
        var shift = await _workShiftRepository.GetByIdAsync(id);
        if (shift == null)
            return null;

        var dto = _mapper.Map<WorkShiftDto>(shift);
        dto.DurationMinutes = shift.GetDurationMinutes();
        return dto;
    }

    public async Task<WorkShiftDto> CreateWorkShiftAsync(CreateWorkShiftRequest request)
    {
        // Validate time
        if (request.StartTime >= request.EndTime)
        {
            throw new ValidationException("Start time must be before end time.");
        }

        var shift = _mapper.Map<WorkShift>(request);
        shift.Created = DateTime.UtcNow;
        var createdShift = await _workShiftRepository.AddAsync(shift);

        var dto = _mapper.Map<WorkShiftDto>(createdShift);
        dto.DurationMinutes = createdShift.GetDurationMinutes();
        return dto;
    }

    public async Task<WorkShiftDto> UpdateWorkShiftAsync(int id, UpdateWorkShiftRequest request)
    {
        var existingShift = await _workShiftRepository.GetByIdAsync(id);
        if (existingShift == null)
        {
            throw new NotFoundException($"WorkShift with ID {id} not found.");
        }

        // Validate time
        if (request.StartTime >= request.EndTime)
        {
            throw new ValidationException("Start time must be before end time.");
        }

        _mapper.Map(request, existingShift);
        existingShift.LastModified = DateTime.UtcNow;
        await _workShiftRepository.UpdateAsync(existingShift);

        var dto = _mapper.Map<WorkShiftDto>(existingShift);
        dto.DurationMinutes = existingShift.GetDurationMinutes();
        return dto;
    }

    public async Task<bool> DeleteWorkShiftAsync(int id)
    {
        var existingShift = await _workShiftRepository.GetByIdAsync(id);
        if (existingShift == null)
        {
            return false;
        }

        await _workShiftRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<WorkShiftDto>> GetActiveWorkShiftsAsync()
    {
        var shifts = await _workShiftRepository.GetActiveShiftsAsync();
        var dtos = _mapper.Map<IEnumerable<WorkShiftDto>>(shifts);

        // Calculate duration minutes
        foreach (var dto in dtos)
        {
            var shift = shifts.First(s => s.Id == dto.Id);
            dto.DurationMinutes = shift.GetDurationMinutes();
        }

        return dtos;
    }

    public async Task<IEnumerable<WorkShiftDto>> SearchWorkShiftsAsync(
        WorkShiftSearchRequest request
    )
    {
        var shifts = await _workShiftRepository.SearchAsync(
            request.Keyword,
            request.IsActive,
            request.Page,
            request.PageSize
        );

        var dtos = _mapper.Map<IEnumerable<WorkShiftDto>>(shifts);

        // Calculate duration minutes
        foreach (var dto in dtos)
        {
            var shift = shifts.First(s => s.Id == dto.Id);
            dto.DurationMinutes = shift.GetDurationMinutes();
        }

        return dtos;
    }
}
