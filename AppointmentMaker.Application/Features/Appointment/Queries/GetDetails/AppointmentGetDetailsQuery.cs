using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Appointment.Queries.GetDetails;

public record AppointmentGetDetailsQuery(Guid Id) : IResultRequest<AppointmentDetailsDto>;
