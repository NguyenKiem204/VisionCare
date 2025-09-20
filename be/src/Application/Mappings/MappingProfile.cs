using AutoMapper;
using VisionCare.Application.DTOs;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null)
            );

        CreateMap<Doctor, DoctorDto>()
            .ForMember(
                dest => dest.SpecializationName,
                opt =>
                    opt.MapFrom(src =>
                        src.Specialization != null ? src.Specialization.SpecializationName : null
                    )
            );

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(
                dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.DoctorName : null)
            )
            .ForMember(
                dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Patient != null ? src.Patient.CustomerName : null)
            );
    }
}
