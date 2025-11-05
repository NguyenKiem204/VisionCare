using AutoMapper;
using VisionCare.Application.DTOs.DoctorScheduleDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Doctors;
using VisionCare.Application.Interfaces.Equipment;
using VisionCare.Application.Interfaces.Rooms;
using VisionCare.Application.Interfaces.Scheduling;
using VisionCare.Application.Interfaces.WorkShifts;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Scheduling;

public class DoctorScheduleService : IDoctorScheduleService
{
    private readonly IDoctorScheduleRepository _doctorScheduleRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IWorkShiftRepository _workShiftRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public DoctorScheduleService(
        IDoctorScheduleRepository doctorScheduleRepository,
        IDoctorRepository doctorRepository,
        IWorkShiftRepository workShiftRepository,
        IRoomRepository roomRepository,
        IEquipmentRepository equipmentRepository,
        IMapper mapper
    )
    {
        _doctorScheduleRepository = doctorScheduleRepository;
        _doctorRepository = doctorRepository;
        _workShiftRepository = workShiftRepository;
        _roomRepository = roomRepository;
        _equipmentRepository = equipmentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorScheduleDto>> GetAllDoctorSchedulesAsync()
    {
        var schedules = await _doctorScheduleRepository.GetAllAsync();
        return await MapToDtosAsync(schedules);
    }

    public async Task<DoctorScheduleDto?> GetDoctorScheduleByIdAsync(int id)
    {
        var schedule = await _doctorScheduleRepository.GetByIdAsync(id);
        if (schedule == null)
            return null;

        var dtos = await MapToDtosAsync(new[] { schedule });
        return dtos.FirstOrDefault();
    }

    public async Task<IEnumerable<DoctorScheduleDto>> GetDoctorSchedulesByDoctorIdAsync(int doctorId)
    {
        var schedules = await _doctorScheduleRepository.GetByDoctorIdAsync(doctorId);
        return await MapToDtosAsync(schedules);
    }

    public async Task<IEnumerable<DoctorScheduleDto>> GetActiveDoctorSchedulesByDoctorIdAsync(int doctorId)
    {
        var schedules = await _doctorScheduleRepository.GetActiveByDoctorIdAsync(doctorId);
        return await MapToDtosAsync(schedules);
    }

    public async Task<DoctorScheduleDto> CreateDoctorScheduleAsync(CreateDoctorScheduleRequest request)
    {
        // Validate doctor exists
        var doctor = await _doctorRepository.GetByIdAsync(request.DoctorId);
        if (doctor == null)
        {
            throw new NotFoundException($"Doctor with ID {request.DoctorId} not found.");
        }

        // Validate shift exists
        var shift = await _workShiftRepository.GetByIdAsync(request.ShiftId);
        if (shift == null)
        {
            throw new NotFoundException($"WorkShift with ID {request.ShiftId} not found.");
        }

        // Validate room if provided
        if (request.RoomId.HasValue)
        {
            var room = await _roomRepository.GetByIdAsync(request.RoomId.Value);
            if (room == null)
            {
                throw new NotFoundException($"Room with ID {request.RoomId.Value} not found.");
            }
        }

        // Validate equipment if provided
        if (request.EquipmentId.HasValue)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId.Value);
            if (equipment == null)
            {
                throw new NotFoundException($"Equipment with ID {request.EquipmentId.Value} not found.");
            }
        }

        // Check for conflicts
        var hasConflict = await _doctorScheduleRepository.ExistsConflictAsync(
            request.DoctorId,
            request.ShiftId,
            request.StartDate,
            request.EndDate,
            request.DayOfWeek
        );

        if (hasConflict)
        {
            throw new ValidationException("A conflicting schedule already exists for this doctor, shift, and time period.");
        }

        var schedule = _mapper.Map<DoctorSchedule>(request);
        schedule.Created = DateTime.UtcNow;
        var createdSchedule = await _doctorScheduleRepository.AddAsync(schedule);
        
        var dtos = await MapToDtosAsync(new[] { createdSchedule });
        return dtos.First();
    }

    public async Task<DoctorScheduleDto> UpdateDoctorScheduleAsync(int id, UpdateDoctorScheduleRequest request)
    {
        var existingSchedule = await _doctorScheduleRepository.GetByIdAsync(id);
        if (existingSchedule == null)
        {
            throw new NotFoundException($"DoctorSchedule with ID {id} not found.");
        }

        // Validate shift exists
        var shift = await _workShiftRepository.GetByIdAsync(request.ShiftId);
        if (shift == null)
        {
            throw new NotFoundException($"WorkShift with ID {request.ShiftId} not found.");
        }

        // Validate room if provided
        if (request.RoomId.HasValue)
        {
            var room = await _roomRepository.GetByIdAsync(request.RoomId.Value);
            if (room == null)
            {
                throw new NotFoundException($"Room with ID {request.RoomId.Value} not found.");
            }
        }

        // Validate equipment if provided
        if (request.EquipmentId.HasValue)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(request.EquipmentId.Value);
            if (equipment == null)
            {
                throw new NotFoundException($"Equipment with ID {request.EquipmentId.Value} not found.");
            }
        }

        // Check for conflicts (excluding current schedule)
        var hasConflict = await _doctorScheduleRepository.ExistsConflictAsync(
            existingSchedule.DoctorId,
            request.ShiftId,
            request.StartDate,
            request.EndDate,
            request.DayOfWeek,
            id
        );

        if (hasConflict)
        {
            throw new ValidationException("A conflicting schedule already exists for this doctor, shift, and time period.");
        }

        _mapper.Map(request, existingSchedule);
        existingSchedule.LastModified = DateTime.UtcNow;
        await _doctorScheduleRepository.UpdateAsync(existingSchedule);
        
        var dtos = await MapToDtosAsync(new[] { existingSchedule });
        return dtos.First();
    }

    public async Task<bool> DeleteDoctorScheduleAsync(int id)
    {
        var existingSchedule = await _doctorScheduleRepository.GetByIdAsync(id);
        if (existingSchedule == null)
        {
            return false;
        }

        await _doctorScheduleRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> HasConflictAsync(int doctorId, int shiftId, DateOnly startDate, DateOnly? endDate, int? dayOfWeek, int? excludeId = null)
    {
        return await _doctorScheduleRepository.ExistsConflictAsync(
            doctorId,
            shiftId,
            startDate,
            endDate,
            dayOfWeek,
            excludeId
        );
    }

    private Task<IEnumerable<DoctorScheduleDto>> MapToDtosAsync(IEnumerable<DoctorSchedule> schedules)
    {
        var dtos = new List<DoctorScheduleDto>();
        
        foreach (var schedule in schedules)
        {
            var dto = _mapper.Map<DoctorScheduleDto>(schedule);
            
            // Load related data
            if (schedule.Doctor != null)
            {
                dto.DoctorName = schedule.Doctor.DoctorName;
            }
            
            if (schedule.Shift != null)
            {
                dto.ShiftName = schedule.Shift.ShiftName;
                dto.StartTime = schedule.Shift.StartTime;
                dto.EndTime = schedule.Shift.EndTime;
            }
            
            if (schedule.Room != null)
            {
                dto.RoomName = schedule.Room.RoomName;
            }
            
            if (schedule.Equipment != null)
            {
                dto.EquipmentName = schedule.Equipment.Name;
            }
            
            // Map day of week to name
            if (schedule.DayOfWeek.HasValue)
            {
                dto.DayOfWeekName = GetDayOfWeekName(schedule.DayOfWeek.Value);
            }
            
            dtos.Add(dto);
        }
        
        return Task.FromResult<IEnumerable<DoctorScheduleDto>>(dtos);
    }

    private static string GetDayOfWeekName(int dayOfWeek)
    {
        return dayOfWeek switch
        {
            1 => "Monday",
            2 => "Tuesday",
            3 => "Wednesday",
            4 => "Thursday",
            5 => "Friday",
            6 => "Saturday",
            7 => "Sunday",
            _ => "Unknown"
        };
    }
}

