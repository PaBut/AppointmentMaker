using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Create.WithBoolArray;

public record ScheduleCreateWithBoolArrayCommand(bool[] BoolTemplate, string DoctorId) : IResultRequest<Guid>;
