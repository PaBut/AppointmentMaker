using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Extentions;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Create.WithTimeIntervals;

public class ScheduleCreateWithTimeIntervalsCommandHandler : IResultRequestHandler<ScheduleCreateWithTimeIntervalsCommand, Guid>
{
    private readonly ScheduleConfiguration _scheduleConfiguration;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleCreateWithTimeIntervalsCommandHandler(IOptions<ScheduleConfiguration> scheduleConfigurationOptions,
        IScheduleRepository scheduleRepository,
        IUnitOfWork unitOfWork)
    {
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
        _scheduleRepository = scheduleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(ScheduleCreateWithTimeIntervalsCommand command, CancellationToken cancellationToken)
    {
        var validator = new ScheduleCreateWithTimeIntervalsCommandValidator(_scheduleConfiguration);

        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            return Result.Failure<Guid>(Error.FromValidationResult(validationResult));
        }

        byte[] byteTemplate = _scheduleConfiguration
            .GetByteTemplateFromTimeIntervals(command.TimeIntervals);

        var schedule = Domain.Entities.Schedule.Create
            (byteTemplate, command.DoctorId, _scheduleConfiguration);

        await _scheduleRepository.CreateAsync(schedule);

        await _unitOfWork.SaveChangesAsync();

        return schedule.Id;
    }
}
