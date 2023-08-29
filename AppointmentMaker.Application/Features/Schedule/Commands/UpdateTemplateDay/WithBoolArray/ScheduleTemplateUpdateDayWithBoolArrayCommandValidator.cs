using AppointmentMaker.Application.Features.Schedule.Commands.Create;
using AppointmentMaker.Domain.Configuration;
using FluentValidation;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateDay.WithBoolArray;

internal class ScheduleTemplateUpdateDayWithBoolArrayCommandValidator : AbstractValidator<ScheduleTemplateUpdateDayWithBoolArrayCommand>
{
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public ScheduleTemplateUpdateDayWithBoolArrayCommandValidator(ScheduleConfiguration scheduleConfiguration)
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
        return array.Length == _scheduleConfiguration.VisitsInDay;
    }
}
