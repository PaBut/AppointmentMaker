using AppointmentMaker.Application.Features.Schedule.Commands.Create;
using AppointmentMaker.Application.Features.Schedule.Commands.Shared;
using AppointmentMaker.Domain.Configuration;
using FluentValidation;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateDay.WithTimeIntervals;

internal class ScheduleTemplateUpdateDayWithTimeIntervalsCommandValidator
    : BaseUpsertScheduleValidator<ScheduleTemplateUpdateDayWithTimeIntervalsCommand>
{
    public ScheduleTemplateUpdateDayWithTimeIntervalsCommandValidator(ScheduleConfiguration scheduleConfiguration)
        : base(scheduleConfiguration)
    {
    }
}
