using FluentValidation;

namespace AppointmentMaker.Application.Features.Appointement.Queries.GetDoctorAppointments;

public class AppointmentGetAllByDoctorQueryValidator : AbstractValidator<AppointmentGetAllByDoctorQuery>
{
    public AppointmentGetAllByDoctorQueryValidator()
    {
        RuleFor(e => e.DoctorId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Doctor Id must be specified");

        RuleFor(e => e.PageSize)
            .GreaterThan(0);
    }
}
