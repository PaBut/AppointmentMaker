using FluentValidation;

namespace AppointmentMaker.Application.Features.Schedule.Queries.GetFreeTimeSlots;

public class ScheduleGetFreeTimeSlotsQueryValidation : AbstractValidator<ScheduleGetFreeTimeSlotsQuery>
{
    public ScheduleGetFreeTimeSlotsQueryValidation()
    {
        RuleFor(e => e.Date)
            .Must(MustBeInFuture)
            .WithMessage("Date must be in future");
    }

    private static bool MustBeInFuture(DateOnly date)
    {
        return date > DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
