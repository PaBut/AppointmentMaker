using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using FluentValidation;

namespace AppointmentMaker.Application.Features.Appointement.Commands.Create;

internal class AppointmentCreateCommandValidator : AbstractValidator<AppointmentCreateCommand>
{
    private readonly IDoctorService _doctorService;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public AppointmentCreateCommandValidator(IDoctorService doctorService,
        ScheduleConfiguration scheduleConfiguration)
    {
        _doctorService = doctorService;
        _scheduleConfiguration = scheduleConfiguration;

        RuleFor(e => e.DateTime)
            .Must(t => t > DateTime.Now)
            .WithMessage("Appointment time cannot be in past");
        RuleFor(e => e.DateTime)
            .Must(t => (t - DateTime.UtcNow).Days <= _scheduleConfiguration.DayLength)
            .WithMessage($"Appointment time cannot be set in later than {_scheduleConfiguration.DayLength} days");
        RuleFor(e => e.DoctorId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Doctor Id must be specified");
        RuleFor(e => e.DoctorId)
            .MustAsync((id, token) => _doctorService.UserExists(id))
            .WithMessage("Respective doctor not found");
    }
}
