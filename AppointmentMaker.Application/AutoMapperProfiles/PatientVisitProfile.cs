using AppointmentMaker.Application.Features.Appointment.Queries.GetDetails;
using AppointmentMaker.Application.Features.Appointment.Queries.GetDoctorAppointments;
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
