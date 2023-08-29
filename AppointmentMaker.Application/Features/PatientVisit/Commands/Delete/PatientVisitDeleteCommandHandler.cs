using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.PatientVisit.Commands.Delete
{
    public class PatientVisitDeleteCommandHandler : IResultRequestHandler<PatientVisitDeleteCommand>
    {
        private readonly IPatientVisitRepository _patientVisitRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PatientVisitDeleteCommandHandler(IPatientVisitRepository patientVisitRepository,
            IUnitOfWork unitOfWork)
        {
            _patientVisitRepository = patientVisitRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(PatientVisitDeleteCommand command, CancellationToken cancellationToken)
        {
            var patientVisit = await _patientVisitRepository.GetByIdAsync(command.Id);

            if (patientVisit == null)
            {
                return Result.Failure(new Error("PatientVisit.Update", "Patient Visit not found"));
            }

            await _patientVisitRepository.DeleteAsync(patientVisit);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
