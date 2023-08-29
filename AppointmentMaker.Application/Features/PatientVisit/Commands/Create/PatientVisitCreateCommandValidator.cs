using FluentValidation;

namespace AppointmentMaker.Application.Features.PatientVisit.Commands.Create;

public class PatientVisitCreateCommandValidator : AbstractValidator<PatientVisitCreateCommand>
{
    public PatientVisitCreateCommandValidator()
    {
        RuleFor(e => e.VisitResult)
            .NotEmpty()
            .NotNull()
            .WithMessage("Visit Result must be provided");
    }
}
