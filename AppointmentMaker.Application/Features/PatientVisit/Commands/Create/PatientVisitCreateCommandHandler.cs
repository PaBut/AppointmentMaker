using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Enums;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.PatientVisit.Commands.Create;

public class PatientVisitCreateCommandHandler : IResultRequestHandler<PatientVisitCreateCommand, Guid>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientVisitRepository _patientVisitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatientVisitCreateCommandHandler(IAppointmentRepository appointmentRepository,
        IPatientVisitRepository patientVisitRepository,
        IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _patientVisitRepository = patientVisitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(PatientVisitCreateCommand command, CancellationToken cancellationToken)
    {
        var validator = new PatientVisitCreateCommandValidator();

        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            return Result.Failure<Guid>(Error.FromValidationResult(validationResult));
        }

        var appointment = await _appointmentRepository.GetByIdAsync(command.AppointmentId);

        if (appointment == null)
        {
            return Result.Failure<Guid>(Error.NotFound(nameof(Domain.Entities.Appointment)));
        }

        appointment.Status = AppointmentStatus.Completed;

        await _appointmentRepository.UpdateAsync(appointment);

        var patientVisit = new Domain.Entities.PatientVisit
        {
            AppointmentId = appointment.Id,
            PatientProblem = appointment.PatientProblem,
            VisitResult = command.VisitResult,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            DateTime = appointment.DateTime
        };

        await _patientVisitRepository.CreateAsync(patientVisit);

        await _unitOfWork.SaveChangesAsync();

        return patientVisit.Id;
    }
}
