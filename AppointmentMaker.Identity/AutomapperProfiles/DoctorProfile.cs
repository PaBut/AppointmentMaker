using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Identity.Entities.Users;
using AutoMapper;

namespace AppointmentMaker.Identity.AutomapperProfiles;

internal class DoctorProfile : Profile
{
    public DoctorProfile()
    {
        CreateMap<DoctorRegisterRequest, Doctor>();
        CreateMap<Doctor, DoctorDetails>();
        CreateMap<Doctor, DoctorFullDetails>();
    }
}
