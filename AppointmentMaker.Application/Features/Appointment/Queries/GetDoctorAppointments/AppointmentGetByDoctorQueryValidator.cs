using FluentValidation;

namespace AppointmentMaker.Application.Features.Appointment.Queries.GetDoctorAppointments;

public class AppointmentGetByDoctorQueryValidator : AbstractValidator<AppointmentGetByDoctorQuery>
{
    public AppointmentGetByDoctorQueryValidator()
    {
        RuleFor(e => e.DoctorId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Doctor Id must be specified");

        RuleFor(e => e.PageSize)
            .GreaterThan(0);
    }
}
