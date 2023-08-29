﻿using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Extentions;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateDay.WithTimeIntervals;

public class ScheduleTemplateUpdateDayWithTimeIntervalsCommandHandler : IResultRequestHandler<ScheduleTemplateUpdateDayWithTimeIntervalsCommand>
{
    private readonly IDoctorService _doctorService;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public ScheduleTemplateUpdateDayWithTimeIntervalsCommandHandler(IDoctorService doctorService,
        IUnitOfWork unitOfWork,
        IScheduleRepository scheduleRepository,
        IOptions<ScheduleConfiguration> scheduleConfigurationOptions)
    {
        _doctorService = doctorService;
        _unitOfWork = unitOfWork;
        _scheduleRepository = scheduleRepository;
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
    }

    public async Task<Result> Handle(ScheduleTemplateUpdateDayWithTimeIntervalsCommand command, CancellationToken cancellationToken)
    {
        var scheduleResult = await _doctorService.GetDoctorSchedule(command.DoctorId);

        if (scheduleResult.IsFailure)
        {
            return Result.Failure(scheduleResult.Error);
        }

        var schedule = scheduleResult.Value;

        byte[] byteTemplate = _scheduleConfiguration
            .GetByteTemplateFromTimeIntervals(command.TimeIntervals);

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