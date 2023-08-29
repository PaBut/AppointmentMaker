using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Appointement.Commands.Cancel;

public record AppointmentCancelCommand(Guid Id) : IResultRequest;
