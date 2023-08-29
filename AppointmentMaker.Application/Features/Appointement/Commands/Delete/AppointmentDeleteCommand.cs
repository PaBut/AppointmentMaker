using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Appointement.Commands.Delete;

public record AppointmentDeleteCommand(Guid Id) : IResultRequest;
