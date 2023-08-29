using AppointmentMaker.Application.Features.Appointement.Queries.GetDetails;
using AppointmentMaker.Application.Features.Appointement.Queries.GetDoctorAppointments;
using AppointmentMaker.Domain.Entities;
using AutoMapper;

namespace AppointmentMaker.Application.AutoMapperProfiles;

public class PatientVisitProfile : Profile
{
    public PatientVisitProfile()
    {
        CreateMap<Appointment, AppointmentDto>();
        CreateMap<Appointment, AppointmentDetailsDto>();
    }
}
