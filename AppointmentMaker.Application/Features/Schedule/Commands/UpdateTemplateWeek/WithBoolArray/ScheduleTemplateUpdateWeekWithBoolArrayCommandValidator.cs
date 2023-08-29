using AppointmentMaker.Application.Features.Schedule.Commands.Create;
using AppointmentMaker.Domain.Configuration;
using FluentValidation;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateWeek.WithBoolArray;

internal class ScheduleTemplateUpdateWeekWithBoolArrayCommandValidator : AbstractValidator<ScheduleTemplateUpdateWeekWithBoolArrayCommand>
{
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public ScheduleTemplateUpdateWeekWithBoolArrayCommandValidator(ScheduleConfiguration scheduleConfiguration)
    {
        _scheduleConfiguration = scheduleConfiguration;

        RuleFor(e => e.ScheduleTemplate)
            .NotNull()
            .NotEmpty()
            .WithMessage("Template must have a value");

        RuleFor(e => e.ScheduleTemplate)
            .Must(MustHaveProperLength)
            .WithMessage("Provided template array must have proper length");
    }

    private bool MustHaveProperLength(bool[] array)
    {
        return array.Length == _scheduleConfiguration.VisitsInWeek;
    }
}
