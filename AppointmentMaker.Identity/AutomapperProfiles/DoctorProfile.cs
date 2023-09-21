using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.Models.Identity.Authentication;
using AppointmentMaker.Identity.Entities.Users;
using AutoMapper;

namespace AppointmentMaker.Identity.AutomapperProfiles;

internal class DoctorProfile : Profile
{
    public DoctorProfile()
    {
        CreateMap<DoctorRegisterWithBoolArrayRequest, Doctor>();
        CreateMap<Doctor, DoctorDetails>();
        CreateMap<Doctor, DoctorFullDetails>();
    }
}
