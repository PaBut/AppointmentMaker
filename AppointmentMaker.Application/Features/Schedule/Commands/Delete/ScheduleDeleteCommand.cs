using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Delete;

public record ScheduleDeleteCommand(Guid Id) : IResultRequest;
