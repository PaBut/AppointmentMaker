using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Extensions;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateDay.WithBoolArray;

public class ScheduleTemplateUpdateDayWithBoolArrayCommandHandler : IResultRequestHandler<ScheduleTemplateUpdateDayWithBoolArrayCommand>
{
    private readonly IDoctorService _doctorService;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public ScheduleTemplateUpdateDayWithBoolArrayCommandHandler(IDoctorService doctorService,
        IUnitOfWork unitOfWork,
        IScheduleRepository scheduleRepository,
        IOptions<ScheduleConfiguration> scheduleConfigurationOptions)
    {
        _doctorService = doctorService;
        _unitOfWork = unitOfWork;
        _scheduleRepository = scheduleRepository;
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
    }

    public async Task<Result> Handle(ScheduleTemplateUpdateDayWithBoolArrayCommand command, CancellationToken cancellationToken)
    {
        var scheduleResult = await _doctorService.GetDoctorSchedule(command.DoctorId);

        if (scheduleResult.IsFailure)
        {
            return Result.Failure(scheduleResult.Error);
        }

        var schedule = scheduleResult.Value;

        byte[] byteTemplate = command.ScheduleTemplate.ToByteArray();

        int weekDayIndex = _scheduleConfiguration.GetWeekDayIndex(command.WeekDay);

        for (int i = 0; i < _scheduleConfiguration.SlotsInDay; i++)
        {
            schedule.ScheduleTemplate[i + weekDayIndex] = byteTemplate[i];
        }

        await _scheduleRepository.UpdateAsync(schedule);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
