using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Extentions;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateWeek.WithBoolArray;

public class ScheduleTemplateUpdateWeekWithBoolArrayCommandHandler : IResultRequestHandler<ScheduleTemplateUpdateWeekWithBoolArrayCommand>
{
    private readonly IDoctorService _doctorService;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleTemplateUpdateWeekWithBoolArrayCommandHandler(IDoctorService doctorService,
        IUnitOfWork unitOfWork,
        IScheduleRepository scheduleRepository)
    {
        _doctorService = doctorService;
        _unitOfWork = unitOfWork;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<Result> Handle(ScheduleTemplateUpdateWeekWithBoolArrayCommand command, CancellationToken cancellationToken)
    {
        var scheduleResult = await _doctorService.GetDoctorSchedule(command.DoctorId);

        if (scheduleResult.IsFailure)
        {
            return Result.Failure(scheduleResult.Error);
        }

        var schedule = scheduleResult.Value;

        schedule.ScheduleTemplate = command.ScheduleTemplate.ToByteArray();

        await _scheduleRepository.UpdateAsync(schedule);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
