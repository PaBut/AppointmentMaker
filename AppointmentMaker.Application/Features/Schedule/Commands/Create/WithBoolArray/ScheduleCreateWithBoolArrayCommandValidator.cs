using AppointmentMaker.Domain.Configuration;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Create.WithBoolArray;

internal class ScheduleCreateWithBoolArrayCommandValidator : AbstractValidator<ScheduleCreateWithBoolArrayCommand>
{
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public ScheduleCreateWithBoolArrayCommandValidator(ScheduleConfiguration scheduleConfiguration)
    {
        _scheduleConfiguration = scheduleConfiguration;

        RuleFor(e => e.BoolTemplate)
            .NotNull()
            .NotEmpty()
            .WithMessage("Template must have a value");

        RuleFor(e => e.BoolTemplate)
            .Must(MustHaveProperLength)
            .WithMessage("Provided template array must have proper length");
    }

    private bool MustHaveProperLength(bool[] array)
    {
        return array.Length == _scheduleConfiguration.VisitsInWeek;
    }
}
