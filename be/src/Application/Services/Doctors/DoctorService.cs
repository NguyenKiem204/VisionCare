using AutoMapper;
using VisionCare.Application.DTOs.DoctorDto;
using VisionCare.Application.Exceptions;
using VisionCare.Application.Interfaces;
using VisionCare.Application.Interfaces.Doctors;
using VisionCare.Application.Interfaces.Services;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Services.Doctors;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IServiceDetailRepository _serviceDetailRepository;
    private readonly IMapper _mapper;

    public DoctorService(
        IDoctorRepository doctorRepository,
        IUserRepository userRepository,
        IServiceDetailRepository serviceDetailRepository,
        IMapper mapper
    )
    {
        _doctorRepository = doctorRepository;
        _userRepository = userRepository;
        _serviceDetailRepository = serviceDetailRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);
        return doctor != null ? _mapper.Map<DoctorDto>(doctor) : null;
    }

    public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorRequest request)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(request.AccountId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.AccountId} not found.");
        }

        // Check if user already has a doctor profile
        if (user.Doctor != null)
        {
            throw new ValidationException("User already has a doctor profile.");
        }

        // Use AutoMapper to create entity from DTO
        var doctor = _mapper.Map<Doctor>(request);
        doctor.DoctorStatus = "Active";
        doctor.Rating = 0.0;
        doctor.Created = DateTime.UtcNow;

        var createdDoctor = await _doctorRepository.AddAsync(doctor);
        return _mapper.Map<DoctorDto>(createdDoctor);
    }

    public async Task<DoctorDto> UpdateDoctorAsync(int id, UpdateDoctorRequest request)
    {
        var existingDoctor = await _doctorRepository.GetByIdAsync(id);
        if (existingDoctor == null)
        {
            throw new NotFoundException($"Doctor with ID {id} not found.");
        }

        // Use AutoMapper to map request to existing entity
        _mapper.Map(request, existingDoctor);

        await _doctorRepository.UpdateAsync(existingDoctor);
        return _mapper.Map<DoctorDto>(existingDoctor);
    }

    public async Task<bool> DeleteDoctorAsync(int id)
    {
        var existingDoctor = await _doctorRepository.GetByIdAsync(id);
        if (existingDoctor == null)
        {
            return false;
        }

        await _doctorRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecializationAsync(int specializationId)
    {
        var doctors = await _doctorRepository.GetBySpecializationAsync(specializationId);
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<IEnumerable<DoctorDto>> GetDoctorsByServiceAsync(int serviceDetailId)
    {
        // Get service detail to find its specialization
        var serviceDetail = await _serviceDetailRepository.GetByIdAsync(serviceDetailId);
        
        if (serviceDetail == null || serviceDetail.Service == null || !serviceDetail.Service.SpecializationId.HasValue)
        {
            return Enumerable.Empty<DoctorDto>();
        }

        // Get doctors with same specialization as the service
        var doctors = await _doctorRepository.GetBySpecializationAsync(serviceDetail.Service.SpecializationId.Value);
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<IEnumerable<DoctorDto>> GetAvailableDoctorsAsync(DateTime date)
    {
        var doctors = await _doctorRepository.GetAvailableDoctorsAsync(date);
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }

    public async Task<(IEnumerable<DoctorDto> items, int totalCount)> SearchDoctorsAsync(
        string keyword,
        int? specializationId,
        double? minRating,
        int page = 1,
        int pageSize = 10,
        string sortBy = "id",
        bool desc = false
    )
    {
        var (doctors, totalCount) = await _doctorRepository.SearchDoctorsAsync(
            keyword,
            specializationId,
            minRating,
            page,
            pageSize,
            sortBy,
            desc
        );
        return (_mapper.Map<IEnumerable<DoctorDto>>(doctors), totalCount);
    }

    public async Task<DoctorDto> UpdateDoctorRatingAsync(int doctorId, double newRating)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId);
        if (doctor == null)
        {
            throw new NotFoundException($"Doctor with ID {doctorId} not found.");
        }

        // Use domain method to update rating
        doctor.UpdateRating(newRating);

        await _doctorRepository.UpdateAsync(doctor);
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<DoctorDto> UpdateDoctorStatusAsync(int doctorId, string status)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId);
        if (doctor == null)
        {
            throw new NotFoundException($"Doctor with ID {doctorId} not found.");
        }

        // Use domain method to update status
        doctor.UpdateStatus(status);

        await _doctorRepository.UpdateAsync(doctor);
        return _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<int> GetTotalDoctorsCountAsync()
    {
        return await _doctorRepository.GetTotalCountAsync();
    }

    public async Task<double> GetAverageRatingAsync()
    {
        return await _doctorRepository.GetAverageRatingAsync();
    }

    public async Task<IEnumerable<DoctorDto>> GetTopRatedDoctorsAsync(int count = 5)
    {
        var doctors = await _doctorRepository.GetTopRatedDoctorsAsync(count);
        return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
    }
}
