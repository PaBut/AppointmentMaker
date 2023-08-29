using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Extentions;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Shift;

public class ScheduleShiftCommandHandler : IResultRequestHandler<ScheduleShiftCommand>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public ScheduleShiftCommandHandler(IScheduleRepository scheduleRepository, 
        IUnitOfWork unitOfWork,
        IOptions<ScheduleConfiguration> scheduleConfigurationOptions)
    {
        _scheduleRepository = scheduleRepository;
        _unitOfWork = unitOfWork;
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
    }

    public async Task<Result> Handle(ScheduleShiftCommand request, CancellationToken cancellationToken)
    {
        var scheduleList = await _scheduleRepository.GetAllAsync();

        foreach(var schedule in scheduleList)
        {
            byte[] scheduleSlots = schedule.ScheduleSlots;
            int slotsInDay = _scheduleConfiguration.SlotsInDay;

            for (int i = 0; i < scheduleSlots.Length - slotsInDay; i++)
            {
                scheduleSlots[i] = schedule.ScheduleSlots[i + slotsInDay];
            }

            int dayIndex = DateTime.UtcNow.DayOfWeek.WeekOffset() * _scheduleConfiguration.SlotsInDay;

            for(int i = scheduleSlots.Length - slotsInDay; i < scheduleSlots.Length; i++)
            {
                scheduleSlots[i] = schedule.ScheduleTemplate[i % dayIndex];
            }

            await _scheduleRepository.UpdateAsync(schedule);
        }

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
