using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Appointment.Commands.Cancel;

public record AppointmentCancelCommand(Guid Id) : IResultRequest;
