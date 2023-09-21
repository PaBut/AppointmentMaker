using AppointmentMaker.Application.Features.Appointment.Commands.Create;
using AppointmentMaker.Application.Features.Appointment.Queries.GetDetails;
using AppointmentMaker.Application.Features.Appointment.Queries.GetDoctorAppointments;
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
