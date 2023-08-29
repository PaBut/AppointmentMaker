using FluentValidation;

namespace AppointmentMaker.Application.Features.Schedule.Queries.GetFreeDays;

public class ScheduleGetFreeDaysQueryValidator : AbstractValidator<ScheduleGetFreeDaysQuery>
{
    public ScheduleGetFreeDaysQueryValidator()
    {
        RuleFor(e => e)
            .Must(MustBeInFuture)
            .WithMessage("Requested date must be in future");
    }

    private bool MustBeInFuture(ScheduleGetFreeDaysQuery query)
    {
        DateTime timeNow = DateTime.UtcNow;
        return (query.Year > timeNow.Year) || 
            (timeNow.Year == query.Year && (int)query.Month > timeNow.Month);
    }
}
