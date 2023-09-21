using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Appointment.Commands.Delete;

public record AppointmentDeleteCommand(Guid Id) : IResultRequest;
