using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoMapper;

namespace AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisits;

public class PatientVisitGetVisitsQueryHandler : IResultRequestHandler<PatientVisitGetVisitsQuery, List<PatientVisitDto>>
{
    private readonly IPatientVisitRepository _patientVisitRepository;
    private readonly IPatientService _patientService;
    private readonly IMapper _mapper;

    public PatientVisitGetVisitsQueryHandler(IPatientVisitRepository patientVisitRepository,
        IMapper mapper,
        IPatientService patientService)
    {
        _patientVisitRepository = patientVisitRepository;
        _mapper = mapper;
        _patientService = patientService;
    }

    public async Task<Result<List<PatientVisitDto>>> Handle(PatientVisitGetVisitsQuery query, CancellationToken cancellationToken)
    {
        var validator = new PatientVisitGetVisitsQueryValidator(_patientService);
        var validationResult = await validator.ValidateAsync(query);

        if (!validationResult.IsValid)
        {
            return Result.Failure<List<PatientVisitDto>>(Error.FromValidationResult(validationResult));
        }

        var patientHistory = await _patientVisitRepository.GetPatientVisitHistory(query.PatientId);

        if (!patientHistory.Any())
        {
            return Result.Failure<List<PatientVisitDto>>(new Error("PatientVisit.Get",
                "Patient visits of the specified patient not found"));
        }

        List<PatientVisitDto> result = _mapper.Map<List<PatientVisitDto>>(patientHistory);

        return result;
    }
}
