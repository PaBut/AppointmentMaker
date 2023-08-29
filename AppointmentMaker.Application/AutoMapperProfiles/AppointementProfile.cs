using AppointmentMaker.Application.Features.Appointement.Commands.Create;
using AppointmentMaker.Application.Features.Appointement.Queries.GetDetails;
using AppointmentMaker.Application.Features.Appointement.Queries.GetDoctorAppointments;
using AppointmentMaker.Domain.Entities;
using AutoMapper;

namespace AppointmentMaker.Application.AutoMapperProfiles;

internal class AppointementProfile : Profile
{
    public AppointementProfile()
    {
        CreateMap<AppointmentCreateCommand, Appointment>();
        CreateMap<Appointment, AppointmentDto>();
        CreateMap<Appointment, AppointmentDetailsDto>();
    }
}
