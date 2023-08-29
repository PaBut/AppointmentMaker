using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.PatientVisit.Commands.Update
{
    public class PatientVisitUpdateCommandHandler : IResultRequestHandler<PatientVisitUpdateCommand>
    {
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PatientVisitUpdateCommandHandler(IPatientVisitRepository patientVisitRepository,
            IUnitOfWork unitOfWork)
        {
            _patientVisitRepository = patientVisitRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(PatientVisitUpdateCommand command, CancellationToken cancellationToken)
        {
            var validator = new PatientVisitUpdateCommandValidator();

            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                return Result.Failure(Error.FromValidationResult(validationResult));
            }

            var patientVisit = await _patientVisitRepository.GetByIdAsync(command.Id);

            if (patientVisit == null)
            {
                return Result.Failure(new Error("PatientVisit.Update", "Patient Visit not found"));
            }

            patientVisit.VisitResult = command.VisitResult;

            await _patientVisitRepository.UpdateAsync(patientVisit);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
