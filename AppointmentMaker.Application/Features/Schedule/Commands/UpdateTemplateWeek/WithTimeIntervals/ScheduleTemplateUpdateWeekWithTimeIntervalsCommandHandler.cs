using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Extensions;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateWeek.WithTimeIntervals;

public class ScheduleTemplateUpdateWeekWithTimeIntervalsCommandHandler : IResultRequestHandler<ScheduleTemplateUpdateWeekWithTimeIntervalsCommand>
{
    private readonly IDoctorService _doctorService;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public ScheduleTemplateUpdateWeekWithTimeIntervalsCommandHandler(IDoctorService doctorService,
        IUnitOfWork unitOfWork,
        IScheduleRepository scheduleRepository,
        IOptions<ScheduleConfiguration> scheduleConfigurationOptions)
    {
        _doctorService = doctorService;
        _unitOfWork = unitOfWork;
        _scheduleRepository = scheduleRepository;
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
    }

    public async Task<Result> Handle(ScheduleTemplateUpdateWeekWithTimeIntervalsCommand command, CancellationToken cancellationToken)
    {
        var scheduleResult = await _doctorService.GetDoctorSchedule(command.DoctorId);

        if (scheduleResult.IsFailure)
        {
            return Result.Failure(scheduleResult.Error);
        }
        var schedule = scheduleResult.Value;

        schedule.ScheduleTemplate = _scheduleConfiguration
            .GetByteTemplateFromTimeIntervals(command.TimeIntervals);

        await _scheduleRepository.UpdateAsync(schedule);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
