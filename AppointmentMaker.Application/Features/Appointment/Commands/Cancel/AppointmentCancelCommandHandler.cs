using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Appointment.Commands.Cancel;

public class AppointmentCancelCommandHandler : IResultRequestHandler<AppointmentCancelCommand>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public AppointmentCancelCommandHandler
        (IOptions<ScheduleConfiguration> scheduleConfigurationOptions,
        IScheduleRepository scheduleRepository,
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
        _scheduleRepository = scheduleRepository;
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AppointmentCancelCommand command, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(command.Id);

        if (appointment == null)
        {
            return Result.Failure(Error.NotFound(nameof(Domain.Entities.Appointment)));
        }

        var appointmentCancelResult = appointment.Cancel();
        
        if (appointmentCancelResult.IsFailure)
        {
            return Result.Failure(appointmentCancelResult.Error);
        }

        await _appointmentRepository.UpdateAsync(appointment);

        var schedule = await _scheduleRepository.GetByIdAsync(appointment.ScheduleId);

        if (schedule == null)
        {
            return Result.Failure(new Error("Error.Unexpected", "Unexpected error"));
        }

        var scheduleChangeResult = schedule.UpdateTimeSlot(appointment.DateTime, false, _scheduleConfiguration);
        
        if (scheduleChangeResult.IsFailure)
        {
            return Result.Failure(scheduleChangeResult.Error);
        }

        await _scheduleRepository.UpdateAsync(schedule);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
