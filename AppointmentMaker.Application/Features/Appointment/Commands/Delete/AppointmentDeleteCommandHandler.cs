using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.Appointment.Commands.Delete;

public class AppointmentDeleteCommandHandler : IResultRequestHandler<AppointmentDeleteCommand>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentDeleteCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Result> Handle(AppointmentDeleteCommand command, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(command.Id);

        if(appointment == null)
        {
            return Result.Failure(Error.NotFound(nameof(Domain.Entities.Appointment)));
        }

        await _appointmentRepository.DeleteAsync(appointment);

        return Result.Success();
    }
}
