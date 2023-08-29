using AppointmentMaker.Application.Features.Schedule.Commands.Shared;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Models;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Create.WithTimeIntervals;

internal class ScheduleCreateWithTimeIntervalsCommandValidator 
    : UpsertWeekScheduleValidator<ScheduleCreateWithTimeIntervalsCommand>
{
    public ScheduleCreateWithTimeIntervalsCommandValidator
        (ScheduleConfiguration scheduleConfiguration) : base(scheduleConfiguration)
    {
    }
}
