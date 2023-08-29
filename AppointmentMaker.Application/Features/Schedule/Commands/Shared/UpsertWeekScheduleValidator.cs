using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Models;
using FluentValidation;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Shared;

public class UpsertWeekScheduleValidator<TCommand>
    : BaseUpsertScheduleValidator<TCommand>
    where TCommand : BaseUpsertScheduleCommand
{
    public UpsertWeekScheduleValidator(ScheduleConfiguration scheduleConfiguration) : base(scheduleConfiguration)
    {
        RuleFor(e => e.TimeIntervals)
            .Must(MustBeOnSameDay)
            .WithMessage("Time Interval must start and end on the same day");
    }

    private bool MustBeOnSameDay(TimeInterval[] timeIntervals)
    {
        return timeIntervals.All(interval => interval.End.DayOfWeek == interval.Start.DayOfWeek);
    }
}
