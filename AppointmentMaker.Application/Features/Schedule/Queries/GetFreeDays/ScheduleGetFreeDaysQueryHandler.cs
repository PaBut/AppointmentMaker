using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Schedule.Queries.GetFreeDays;

public class ScheduleGetFreeDaysQueryHandler : IResultRequestHandler<ScheduleGetFreeDaysQuery, List<int>>
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public ScheduleGetFreeDaysQueryHandler(IScheduleRepository scheduleRepository,
        IOptions<ScheduleConfiguration> scheduleConfigurationOptions)
    {
        _scheduleRepository = scheduleRepository;
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
    }

    public async Task<Result<List<int>>> Handle(ScheduleGetFreeDaysQuery query, CancellationToken cancellationToken)
    {
        ScheduleGetFreeDaysQueryValidator validator = new ScheduleGetFreeDaysQueryValidator();
        var validationResult = await validator.ValidateAsync(query);

        if (!validationResult.IsValid)
        {
            return Result.Failure<List<int>>(Error.FromValidationResult(validationResult));
        }

        var schedule = await _scheduleRepository.GetByIdAsync(query.Id);

        if (schedule == null)
        {
            return Result.Failure<List<int>>(new Error("Schedule.Get", "Schedule wiith specified id not found"));
        }

        int startIndex = _scheduleConfiguration.GetMonthIndex((int)query.Month, query.Year);
        int endIndex = startIndex + _scheduleConfiguration.MonthSlotCount((int)query.Month, query.Year);

        HashSet<int> freeDates = new HashSet<int>();

        for(int i = startIndex; i < endIndex; i++)
        {
            if (schedule.ScheduleSlots[i] != 1)
            {
                int date = (i - startIndex) / _scheduleConfiguration.SlotsInDay + 1;
                freeDates.Add(date);
            }
        }

        return freeDates.ToList();
    }
}
