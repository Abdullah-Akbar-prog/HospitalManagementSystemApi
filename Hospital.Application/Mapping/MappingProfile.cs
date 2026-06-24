using AutoMapper;
using Hospital.Application.DTOs;
using Hospital.Domain.Entities;

namespace Hospital.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>();
            CreateMap<PatientDto, Patient>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Ignore());

            CreateMap<Doctor, DoctorDto>();
            CreateMap<DoctorDto, Doctor>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.UserId, opt => opt.Ignore());

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));
        }

    }
}
