using AutoMapper;
using VisionCare.Application.DTOs;
using VisionCare.Application.DTOs.AppointmentDto;
using VisionCare.Application.DTOs.CustomerDto;
using VisionCare.Application.DTOs.DoctorDto;
using VisionCare.Application.DTOs.FeedbackDto;
using VisionCare.Application.DTOs.MedicalHistoryDto;
using VisionCare.Application.DTOs.RoleDto;
using VisionCare.Application.DTOs.ScheduleDto;
using VisionCare.Application.DTOs.ServiceDetailDto;
using VisionCare.Application.DTOs.ServiceDto;
using VisionCare.Application.DTOs.SlotDto;
using VisionCare.Application.DTOs.SpecializationDto;
using VisionCare.Application.DTOs.StaffDto;
using VisionCare.Application.DTOs.User;
using VisionCare.Domain.Entities;

namespace VisionCare.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null)
            );

        // Doctor mappings
        CreateMap<Doctor, DoctorDto>()
            .ForMember(
                dest => dest.SpecializationName,
                opt =>
                    opt.MapFrom(src =>
                        src.Specialization != null ? src.Specialization.SpecializationName : null
                    )
            )
            .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Account != null ? src.Account.Email : null)
            )
            .ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.Account != null ? src.Account.Username : null)
            );

        // Customer mappings
        CreateMap<Customer, CustomerDto>()
            .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Account != null ? src.Account.Email : null)
            )
            .ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.Account != null ? src.Account.Username : null)
            )
            .ForMember(
                dest => dest.Age,
                opt =>
                    opt.MapFrom(src =>
                        src.Dob.HasValue
                            ? DateTime.Today.Year
                                - src.Dob.Value.Year
                                - (DateTime.Today.DayOfYear < src.Dob.Value.DayOfYear ? 1 : 0)
                            : (int?)null
                    )
            );

        // Appointment mappings
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(
                dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.DoctorName : null)
            )
            .ForMember(
                dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Patient != null ? src.Patient.CustomerName : null)
            )
            .ForMember(
                dest => dest.SpecializationName,
                opt =>
                    opt.MapFrom(src =>
                        src.Doctor != null && src.Doctor.Specialization != null
                            ? src.Doctor.Specialization.SpecializationName
                            : null
                    )
            );

        // Specialization mappings
        CreateMap<Specialization, SpecializationDto>();

        // Staff mappings
        CreateMap<Staff, StaffDto>()
            .ForMember(
                dest => dest.Email,
                opt => opt.MapFrom(src => src.Account != null ? src.Account.Email : null)
            )
            .ForMember(
                dest => dest.Username,
                opt => opt.MapFrom(src => src.Account != null ? src.Account.Username : null)
            )
            .ForMember(
                dest => dest.Age,
                opt =>
                    opt.MapFrom(src =>
                        src.Dob.HasValue
                            ? DateTime.Today.Year
                                - src.Dob.Value.Year
                                - (DateTime.Today.DayOfYear < src.Dob.Value.DayOfYear ? 1 : 0)
                            : (int?)null
                    )
            );

        // Role mappings
        CreateMap<Role, RoleDto>();

        // DTO to Entity mappings for Clean Architecture
        CreateMap<CreateUserRequest, User>();
        CreateMap<UpdateUserRequest, User>();

        CreateMap<CreateDoctorRequest, Doctor>();
        CreateMap<UpdateDoctorRequest, Doctor>();

        CreateMap<CreateCustomerRequest, Customer>();
        CreateMap<UpdateCustomerRequest, Customer>();
        CreateMap<UpdateCustomerProfileRequest, Customer>();

        CreateMap<CreateStaffRequest, Staff>();
        CreateMap<UpdateStaffRequest, Staff>();
        CreateMap<UpdateStaffProfileRequest, Staff>();

        CreateMap<CreateAppointmentRequest, Appointment>();
        CreateMap<UpdateAppointmentRequest, Appointment>();

        // Service mappings
        CreateMap<Service, ServiceDto>()
            .ForMember(
                dest => dest.SpecializationName,
                opt =>
                    opt.MapFrom(src =>
                        src.Specialization != null ? src.Specialization.SpecializationName : null
                    )
            )
            .ForMember(
                dest => dest.MinPrice,
                opt =>
                    opt.MapFrom(src =>
                        src.ServiceDetails != null && src.ServiceDetails.Any()
                            ? src.ServiceDetails.Min(sd => sd.Cost)
                            : (decimal?)null
                    )
            )
            .ForMember(
                dest => dest.MaxPrice,
                opt =>
                    opt.MapFrom(src =>
                        src.ServiceDetails != null && src.ServiceDetails.Any()
                            ? src.ServiceDetails.Max(sd => sd.Cost)
                            : (decimal?)null
                    )
            )
            .ForMember(
                dest => dest.MinDuration,
                opt =>
                    opt.MapFrom(src =>
                        src.ServiceDetails != null && src.ServiceDetails.Any()
                            ? src.ServiceDetails.Min(sd =>
                                sd.ServiceType != null ? sd.ServiceType.DurationMinutes : 0
                            )
                            : (int?)null
                    )
            )
            .ForMember(
                dest => dest.MaxDuration,
                opt =>
                    opt.MapFrom(src =>
                        src.ServiceDetails != null && src.ServiceDetails.Any()
                            ? src.ServiceDetails.Max(sd =>
                                sd.ServiceType != null ? sd.ServiceType.DurationMinutes : 0
                            )
                            : (int?)null
                    )
            )
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == "Active"));

        // ServiceDetail mappings
        CreateMap<ServiceDetail, ServiceDetailDto>()
            .ForMember(
                dest => dest.ServiceName,
                opt => opt.MapFrom(src => src.Service != null ? src.Service.Name : null)
            )
            .ForMember(
                dest => dest.ServiceTypeName,
                opt => opt.MapFrom(src => src.ServiceType != null ? src.ServiceType.Name : null)
            );

        // DTO to Entity mappings for Services
        CreateMap<CreateServiceRequest, Service>();
        CreateMap<UpdateServiceRequest, Service>();

        // Infrastructure mappings are defined in Infrastructure layer

        // MedicalHistory mappings
        CreateMap<MedicalHistory, MedicalHistoryDto>()
            .ForMember(
                dest => dest.PatientName,
                opt =>
                    opt.MapFrom(src =>
                        src.Appointment != null && src.Appointment.Patient != null
                            ? src.Appointment.Patient.CustomerName
                            : null
                    )
            )
            .ForMember(
                dest => dest.DoctorName,
                opt =>
                    opt.MapFrom(src =>
                        src.Appointment != null && src.Appointment.Doctor != null
                            ? src.Appointment.Doctor.DoctorName
                            : null
                    )
            )
            .ForMember(
                dest => dest.AppointmentDate,
                opt =>
                    opt.MapFrom(src =>
                        src.Appointment != null ? src.Appointment.AppointmentDate : null
                    )
            );

        // DTO to Entity mappings for MedicalHistory
        CreateMap<CreateMedicalHistoryRequest, MedicalHistory>();
        CreateMap<UpdateMedicalHistoryRequest, MedicalHistory>();

        // Infrastructure mappings are defined in Infrastructure layer

        // Slot mappings
        CreateMap<Slot, SlotDto>()
            .ForMember(
                dest => dest.ServiceTypeName,
                opt => opt.MapFrom(src => src.ServiceType != null ? src.ServiceType.Name : null)
            )
            .ForMember(
                dest => dest.DurationMinutes,
                opt => opt.MapFrom(src => src.GetDurationMinutes())
            );

        CreateMap<CreateSlotRequest, Slot>();
        CreateMap<UpdateSlotRequest, Slot>();

        // Schedule mappings
        CreateMap<Schedule, ScheduleDto>()
            .ForMember(
                dest => dest.DoctorName,
                opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.DoctorName : null)
            )
            .ForMember(
                dest => dest.StartTime,
                opt => opt.MapFrom(src => src.Slot != null ? src.Slot.StartTime : TimeOnly.MinValue)
            )
            .ForMember(
                dest => dest.EndTime,
                opt => opt.MapFrom(src => src.Slot != null ? src.Slot.EndTime : TimeOnly.MinValue)
            );

        CreateMap<CreateScheduleRequest, Schedule>();
        CreateMap<UpdateScheduleRequest, Schedule>();

        // Infrastructure mappings are defined in Infrastructure layer

        // Infrastructure mappings are defined in Infrastructure layer

        // FeedbackDoctor mappings
        CreateMap<FeedbackDoctor, FeedbackDoctorDto>()
            .ForMember(
                dest => dest.PatientName,
                opt =>
                    opt.MapFrom(src =>
                        src.Appointment != null && src.Appointment.Patient != null
                            ? src.Appointment.Patient.CustomerName
                            : null
                    )
            )
            .ForMember(
                dest => dest.DoctorName,
                opt =>
                    opt.MapFrom(src =>
                        src.Appointment != null && src.Appointment.Doctor != null
                            ? src.Appointment.Doctor.DoctorName
                            : null
                    )
            )
            .ForMember(
                dest => dest.AppointmentDate,
                opt =>
                    opt.MapFrom(src =>
                        src.Appointment != null ? src.Appointment.AppointmentDate : null
                    )
            );

        CreateMap<CreateFeedbackDoctorRequest, FeedbackDoctor>();
        CreateMap<UpdateFeedbackDoctorRequest, FeedbackDoctor>();

        // FeedbackService mappings
        CreateMap<VisionCare.Domain.Entities.FeedbackService, FeedbackServiceDto>()
            .ForMember(
                dest => dest.PatientName,
                opt =>
                    opt.MapFrom(src =>
                        src.Appointment != null && src.Appointment.Patient != null
                            ? src.Appointment.Patient.CustomerName
                            : null
                    )
            )
            // Appointment does not expose ServiceDetail in Domain; resolve at query layer if needed
            .ForMember(
                dest => dest.AppointmentDate,
                opt =>
                    opt.MapFrom(src =>
                        src.Appointment != null ? src.Appointment.AppointmentDate : null
                    )
            );

        CreateMap<CreateFeedbackServiceRequest, VisionCare.Domain.Entities.FeedbackService>();
        CreateMap<UpdateFeedbackServiceRequest, VisionCare.Domain.Entities.FeedbackService>();

        // Infrastructure mappings are defined in Infrastructure layer
    }
}
