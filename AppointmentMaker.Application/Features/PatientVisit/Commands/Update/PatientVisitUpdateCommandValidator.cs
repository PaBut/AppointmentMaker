using FluentValidation;

namespace AppointmentMaker.Application.Features.PatientVisit.Commands.Update;

public class PatientVisitUpdateCommandValidator : AbstractValidator<PatientVisitUpdateCommand>
{
    public PatientVisitUpdateCommandValidator()
    {
        RuleFor(e => e.VisitResult)
            .NotEmpty()
            .NotNull()
            .WithMessage("Visit Result must be provided");
    }
}
