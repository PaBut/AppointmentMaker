using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoMapper;

namespace AppointmentMaker.Application.Features.Appointement.Queries.GetDetails;

public class AppointmentGetDetailsQueryHandler 
    : IResultRequestHandler<AppointmentGetDetailsQuery, AppointmentDetailsDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientService _patientService;
    private readonly IMapper _mapper;

    public AppointmentGetDetailsQueryHandler(IAppointmentRepository appointmentRepository,
        IMapper mapper,
        IPatientService patientService)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
        _patientService = patientService;
    }

    public async Task<Result<AppointmentDetailsDto>> Handle
        (AppointmentGetDetailsQuery query, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(query.Id);

        if(appointment == null)
        {
            return Result.Failure<AppointmentDetailsDto>(new Error("Appointment.Get",
                "Appointment with specified id not found"));
        }

        AppointmentDetailsDto detailsDto = _mapper.Map<AppointmentDetailsDto>(appointment);

        var patientResult = await _patientService.GetDetails(appointment.PatientId);

        if (!patientResult.IsFailure)
        {
            return Result.Failure<AppointmentDetailsDto>(patientResult.Error);
        }

        var patient = patientResult.Value;

        detailsDto.PatientFullName = patient.FullName;
        detailsDto.PatientEmail = patient.Email;

        return detailsDto;
    }
}
