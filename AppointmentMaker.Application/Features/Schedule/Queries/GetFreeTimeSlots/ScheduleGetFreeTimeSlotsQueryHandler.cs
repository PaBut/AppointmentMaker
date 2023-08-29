using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Queries.GetFreeTimeSlots;

public class ScheduleGetFreeTimeSlotsQueryHandler : IResultRequestHandler<ScheduleGetFreeTimeSlotsQuery, List<TimeOnly>>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public ScheduleGetFreeTimeSlotsQueryHandler(IScheduleRepository scheduleRepository,
        IOptions<ScheduleConfiguration> scheduleConfigurationOptions)
    {
        _scheduleRepository = scheduleRepository;
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
    }

    public async Task<Result<List<TimeOnly>>> Handle(ScheduleGetFreeTimeSlotsQuery query, CancellationToken cancellationToken)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(query.Id);

        if (schedule == null)
        {
            return Result.Failure<List<TimeOnly>>(new Error("Schedule.Get", "Schedule with specified id not found"));
        }

        int dayIndex = _scheduleConfiguration.GetMonthIndex(query.Date.Month, query.Date.Year) +
            _scheduleConfiguration.SlotsInDay * (query.Date.Day - DateTime.UtcNow.Day);

        bool isToday = query.Date == DateOnly.FromDateTime(DateTime.UtcNow);

        List<TimeOnly> result = new List<TimeOnly>();

        for(int i = 0; i < _scheduleConfiguration.SlotsInDay; i++)
        {
            TimeSpan baseTime = _scheduleConfiguration.GetTimeByIndex(i);
            for (int j = 0; j < 8; j++)
            {
                if (((schedule.ScheduleSlots[i + dayIndex] >> j) & 1) == 1)
                {
                    TimeSpan time = baseTime.Add(TimeSpan.FromMinutes(j * _scheduleConfiguration.VisitLengthInMinutes)); 
                    if (!isToday || DateTime.UtcNow.TimeOfDay < time)
                    {
                        result.Add(TimeOnly.FromTimeSpan(time));
                    }
                }
            }
        }

        return result;
    }
}
