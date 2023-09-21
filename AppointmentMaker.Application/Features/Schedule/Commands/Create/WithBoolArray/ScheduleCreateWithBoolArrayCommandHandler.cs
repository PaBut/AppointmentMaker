using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Extensions;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Create.WithBoolArray;

public class ScheduleCreateWithBoolArrayCommandHandler : IResultRequestHandler<ScheduleCreateWithBoolArrayCommand, Guid>
{
    private readonly ScheduleConfiguration _scheduleConfiguration;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleCreateWithBoolArrayCommandHandler(IOptions<ScheduleConfiguration> scheduleConfigurationOptions,
        IScheduleRepository scheduleRepository,
        IUnitOfWork unitOfWork)
    {
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
        _scheduleRepository = scheduleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(ScheduleCreateWithBoolArrayCommand command, CancellationToken cancellationToken)
    {
        var validator = new ScheduleCreateWithBoolArrayCommandValidator(_scheduleConfiguration);

        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            return Result.Failure<Guid>(Error.FromValidationResult(validationResult));
        }

        byte[] byteTemplate = command.BoolTemplate.ToByteArray();

        var schedule = Domain.Entities.Schedule.Create
            (byteTemplate, command.DoctorId, _scheduleConfiguration);

        await _scheduleRepository.CreateAsync(schedule);

        await _unitOfWork.SaveChangesAsync();

        return schedule.Id;
    }
}
