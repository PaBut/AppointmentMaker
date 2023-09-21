using AppointmentMaker.Application.ServiceContracts;
using FluentValidation;

namespace AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisits;

public class PatientVisitGetVisitsQueryValidator : AbstractValidator<PatientVisitGetVisitsQuery>
{
    private readonly IPatientService _patientService;

    public PatientVisitGetVisitsQueryValidator(IPatientService patientService)
    {
        _patientService = patientService;

        RuleFor(e => e.PatientId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Patient Id must be specified");

        RuleFor(e => e.PatientId)
            .MustAsync(PatientExists)
            .WithMessage("Patient with specified id does not exist")
            .WithErrorCode("Error.NotFound");
    }

    private async Task<bool> PatientExists(string id, CancellationToken token)
    {
        return await _patientService.UserExists(id);
    }
}
