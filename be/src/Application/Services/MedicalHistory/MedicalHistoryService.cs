using AutoMapper;
using VisionCare.Application.DTOs.MedicalHistoryDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces.MedicalHistory;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.MedicalHistory;

public class MedicalHistoryService : IMedicalHistoryService
{
    private readonly IMedicalHistoryRepository _medicalHistoryRepository;
    private readonly IMapper _mapper;

    public MedicalHistoryService(IMedicalHistoryRepository medicalHistoryRepository, IMapper mapper)
    {
        _medicalHistoryRepository = medicalHistoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MedicalHistoryDto>> GetAllMedicalHistoriesAsync()
    {
        var medicalHistories = await _medicalHistoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);
    }

    public async Task<MedicalHistoryDto?> GetMedicalHistoryByIdAsync(int id)
    {
        var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(id);
        return medicalHistory != null ? _mapper.Map<MedicalHistoryDto>(medicalHistory) : null;
    }

    public async Task<MedicalHistoryDto?> GetMedicalHistoryByAppointmentIdAsync(int appointmentId)
    {
        var medicalHistory = await _medicalHistoryRepository.GetByAppointmentIdAsync(appointmentId);
        return medicalHistory != null ? _mapper.Map<MedicalHistoryDto>(medicalHistory) : null;
    }

    public async Task<IEnumerable<MedicalHistoryDto>> GetMedicalHistoriesByPatientIdAsync(
        int patientId
    )
    {
        var medicalHistories = await _medicalHistoryRepository.GetByPatientIdAsync(patientId);
        return _mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);
    }

    public async Task<IEnumerable<MedicalHistoryDto>> GetMedicalHistoriesByDoctorIdAsync(
        int doctorId
    )
    {
        var medicalHistories = await _medicalHistoryRepository.GetByDoctorIdAsync(doctorId);
        return _mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);
    }

    public async Task<MedicalHistoryDto> CreateMedicalHistoryAsync(
        CreateMedicalHistoryRequest request
    )
    {
        // Check if medical history already exists for this appointment
        if (await _medicalHistoryRepository.ExistsForAppointmentAsync(request.AppointmentId))
        {
            throw new ValidationException("Medical history already exists for this appointment.");
        }

        // Use AutoMapper to create entity from DTO
        var medicalHistory = _mapper.Map<VisionCare.Domain.Entities.MedicalHistory>(request);
        medicalHistory.Created = DateTime.UtcNow;

        var createdMedicalHistory = await _medicalHistoryRepository.AddAsync(medicalHistory);
        return _mapper.Map<MedicalHistoryDto>(createdMedicalHistory);
    }

    public async Task<MedicalHistoryDto> UpdateMedicalHistoryAsync(
        int id,
        UpdateMedicalHistoryRequest request
    )
    {
        var existingMedicalHistory = await _medicalHistoryRepository.GetByIdAsync(id);
        if (existingMedicalHistory == null)
        {
            throw new NotFoundException($"Medical history with ID {id} not found.");
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingMedicalHistory);

        await _medicalHistoryRepository.UpdateAsync(existingMedicalHistory);
        return _mapper.Map<MedicalHistoryDto>(existingMedicalHistory);
    }

    public async Task<bool> DeleteMedicalHistoryAsync(int id)
    {
        var existingMedicalHistory = await _medicalHistoryRepository.GetByIdAsync(id);
        if (existingMedicalHistory == null)
        {
            return false;
        }

        await _medicalHistoryRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<MedicalHistoryDto>> SearchMedicalHistoriesAsync(
        MedicalHistorySearchRequest request
    )
    {
        var medicalHistories = await _medicalHistoryRepository.SearchAsync(
            request.PatientId,
            request.DoctorId,
            request.FromDate,
            request.ToDate,
            request.Diagnosis
        );

        return _mapper.Map<IEnumerable<MedicalHistoryDto>>(medicalHistories);
    }

    public async Task<int> GetTotalMedicalHistoriesCountAsync()
    {
        return await _medicalHistoryRepository.GetTotalCountAsync();
    }

    public async Task<bool> MedicalHistoryExistsForAppointmentAsync(int appointmentId)
    {
        return await _medicalHistoryRepository.ExistsForAppointmentAsync(appointmentId);
    }
}
