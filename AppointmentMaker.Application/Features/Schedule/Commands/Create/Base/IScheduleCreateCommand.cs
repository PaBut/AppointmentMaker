using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Create.Base;

internal interface IScheduleCreateCommand : IResultRequest<Guid>
{
}
