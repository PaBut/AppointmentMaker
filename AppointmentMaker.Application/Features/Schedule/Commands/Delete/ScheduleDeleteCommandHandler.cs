using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Delete;

public class ScheduleDeleteCommandHandler : IResultRequestHandler<ScheduleDeleteCommand>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleDeleteCommandHandler(IScheduleRepository scheduleRepository,
        IUnitOfWork unitOfWork,
        IAppointmentRepository appointmentRepository)
    {
        _scheduleRepository = scheduleRepository;
        _unitOfWork = unitOfWork;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Result> Handle(ScheduleDeleteCommand request, CancellationToken cancellationToken)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(request.Id);

        if (schedule == null)
        {
            return Result.Failure(new Error("Schedule.Delete", "Schedule with specified id not found"));
        }

        await _scheduleRepository.DeleteAsync(schedule);

        var appointments = await _appointmentRepository.GetScheduleAppointmentsAsync(request.Id);

        if (appointments.Any())
        {
            await _appointmentRepository.DeleteRangeAsync(appointments);
        }

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
