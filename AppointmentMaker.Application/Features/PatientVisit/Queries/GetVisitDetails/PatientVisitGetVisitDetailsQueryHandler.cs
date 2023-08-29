using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoMapper;

namespace AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisitDetails;

public class PatientVisitGetVisitDetailsQueryHandler 
    : IResultRequestHandler<PatientVisitGetVisitDetailsQuery, PatientVisitDetailsDto>
{
    private readonly IPatientVisitRepository _patientVisitRepository;
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;

    public PatientVisitGetVisitDetailsQueryHandler(IPatientVisitRepository patientVisitRepository,
        IMapper mapper,
        IDoctorService doctorService)
    {
        _patientVisitRepository = patientVisitRepository;
        _mapper = mapper;
        _doctorService = doctorService;
    }

    public async Task<Result<PatientVisitDetailsDto>> Handle(PatientVisitGetVisitDetailsQuery query,
        CancellationToken cancellationToken)
    {
        var patientVisit = await _patientVisitRepository.GetByIdAsync(query.Id);

        if(patientVisit == null)
        {
            return Result.Failure<PatientVisitDetailsDto>(new Error("PatientVisit.Get",
                "Patient Visit with specified id not found"));
        }

        var detailsDto = _mapper.Map<PatientVisitDetailsDto>(patientVisit);

        var doctorResult = await _doctorService.GetDetails(patientVisit.DoctorId);

        if (doctorResult.IsFailure)
        {
            return Result.Failure<PatientVisitDetailsDto>(doctorResult.Error);
        }

        var doctor = doctorResult.Value;

        detailsDto.DoctorFullName = doctor.FullName;
        detailsDto.DoctorEmail = doctor.Email;

        return detailsDto;
    }
}
