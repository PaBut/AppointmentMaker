using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Models;
using FluentValidation;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Shared;

public class BaseUpsertScheduleValidator<TCommand>
    : AbstractValidator<TCommand>
    where TCommand : BaseUpsertScheduleCommand
{
    protected readonly ScheduleConfiguration _scheduleConfiguration;

    public BaseUpsertScheduleValidator(ScheduleConfiguration scheduleConfiguration)
    {
        _scheduleConfiguration = scheduleConfiguration;

        RuleFor(e => e.DoctorId)
            .NotEmpty()
            .WithMessage("Doctor Id must be specified");

        RuleFor(e => e.TimeIntervals)
            .NotNull()
            .NotEmpty()
            .WithMessage("Time Intervals must have a value");

        RuleFor(e => e.TimeIntervals)
            .Must(TimeIntervalsMustHaveProperLength)
        .WithMessage($"Time Intervals must be divisible by " +
            $"Schedule Slots Time ({_scheduleConfiguration.VisitLengthInMinutes})");

        RuleFor(e => e.TimeIntervals)
            .Must(EndMustBeAfterStart)
            .WithMessage("End in time intervals must be after start");

        RuleFor(e => e.TimeIntervals)
            .Must(MustNotIntersect)
            .WithMessage("Time Intervals must not intersect");
    }

    private bool MustNotIntersect(TimeInterval[] timeIntervals)
    {
        return timeIntervals.All(interval1 =>
        !timeIntervals.Any(interval2 => interval2.IsTimeBetween(interval1.Start)
        || interval2.IsTimeBetween(interval1.End)));
    }

    private bool EndMustBeAfterStart(TimeInterval[] timeIntervals)
    {
        return timeIntervals.All(interval => (interval.End.Time - interval.Start.Time).TotalMinutes > 0);
    }

    private bool TimeIntervalsMustHaveProperLength(TimeInterval[] timeIntervals)
    {
        return timeIntervals.All(interval => (interval.End.Time - interval.Start.Time).TotalMinutes
        % _scheduleConfiguration.VisitLengthInMinutes == 0);
    }
}
