using AppointmentMaker.Application.Features.Schedule.Commands.Create;
using AppointmentMaker.Application.Features.Schedule.Commands.Shared;
using AppointmentMaker.Domain.Configuration;
using FluentValidation;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateWeek.WithTimeIntervals;

internal class ScheduleTemplateUpdateWeekWithTimeIntervalsCommandValidator 
    : UpsertWeekScheduleValidator<ScheduleTemplateUpdateWeekWithTimeIntervalsCommand>
{
    public ScheduleTemplateUpdateWeekWithTimeIntervalsCommandValidator
        (ScheduleConfiguration scheduleConfiguration)
        : base(scheduleConfiguration)
    {
    }
}
