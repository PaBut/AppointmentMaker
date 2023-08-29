using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Identity.Entities.Users;
using AutoMapper;

namespace AppointmentMaker.Identity.AutomapperProfiles;

internal class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<PatientRegisterRequest, Patient>();
        CreateMap<Patient, PatientDetails>();
        CreateMap<Patient, PatientFullDetails>();
    }
}
