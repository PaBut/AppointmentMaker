using AppointmentMaker.Application.Features.Schedule.Commands.Create.WithTimeIntervals;
using AppointmentMaker.Application.Features.Schedule.Queries.GetFreeDays;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Application.UnitTests.Mocks;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Enums;
using AppointmentMaker.Domain.Models;
using AppointmentMaker.Domain.RepositoryContracts;
using Microsoft.Extensions.Options;
using Shouldly;

namespace AppointmentMaker.Application.UnitTests.Features.Schedule.GetFreeDays;

public class ScheduleGetFreeDaysQueryHandlerTests
{
    private readonly IScheduleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleGetFreeDaysQueryHandlerTests()
    {
        _repository = MockScheduleRepository.GetScheduleRepository().Object;
        _unitOfWork = MockUnitOfWork.GetUnitOfWorkMock().Object;
    }

    [Fact]
    public async Task ShouldContainAllFreeDays()
    {
        const DayOfWeek firstDayOfWeek = DayOfWeek.Monday;
        const int firstStartHour = 20;
        const int firstEndHour = 21;

        const DayOfWeek secondDayOfWeek = DayOfWeek.Friday;
        const int secondStartHour = 22;
        const int secondEndHour = 23;

        const int monthsInYear = 12;

        TimeInterval[] intervals = new TimeInterval[]
        {
            new TimeInterval
            {
                Start = new WeekTime
                {
                    DayOfWeek = firstDayOfWeek,
                    Time = new TimeOnly(firstStartHour, 0)
                },
                End = new WeekTime
                {
                    DayOfWeek = firstDayOfWeek,
                    Time = new TimeOnly(firstEndHour, 0)
                }
            },
            new TimeInterval
            {
                Start = new WeekTime
                {
                    DayOfWeek = secondDayOfWeek,
                    Time = new TimeOnly(secondStartHour, 0)
                },
                End = new WeekTime
                {
                    DayOfWeek = secondDayOfWeek,
                    Time = new TimeOnly(secondEndHour, 0)
                }
            }
        };

        ScheduleConfiguration configuration = new ScheduleConfiguration
        {
            VisitLengthInMinutes = 15,
            DayLength = 182
        };

        IOptions<ScheduleConfiguration> options = Options.Create(configuration);

        ScheduleCreateWithTimeIntervalsCommandHandler handler
            = new ScheduleCreateWithTimeIntervalsCommandHandler
            (options, _repository, _unitOfWork);

        var result = await handler.Handle
            (new ScheduleCreateWithTimeIntervalsCommand(intervals, Guid.NewGuid().ToString()),
            CancellationToken.None);

        result.IsSuccess.ShouldBe(true);

        ScheduleGetFreeDaysQueryHandler getHandler = new ScheduleGetFreeDaysQueryHandler(_repository, options);

        int nextMonth = (DateTime.UtcNow.Month + 1) % monthsInYear;
        int year = DateTime.UtcNow.Year + (DateTime.UtcNow.Month + 1) / monthsInYear;

        var getResult = await getHandler.Handle(new ScheduleGetFreeDaysQuery(result.Value,
            (Months)nextMonth,
            year),
            CancellationToken.None);

        getResult.IsSuccess.ShouldBe(true);

        for(int i = 1; i <= DateTime.DaysInMonth(year, nextMonth); i++)
        {
            DateOnly tempDate = new DateOnly(year, nextMonth, i);
            if (intervals.Any(i => i.Start.DayOfWeek == tempDate.DayOfWeek))
            {
                getResult.Value.ShouldContain(tempDate.Day);
            }
        }
    }

}
