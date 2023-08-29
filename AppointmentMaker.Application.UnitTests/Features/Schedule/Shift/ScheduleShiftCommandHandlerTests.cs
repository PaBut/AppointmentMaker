using AppointmentMaker.Application.Features.Schedule.Commands.Create.WithTimeIntervals;
using AppointmentMaker.Application.Features.Schedule.Commands.Shift;
using AppointmentMaker.Application.Features.Schedule.Queries.GetFreeTimeSlots;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Application.UnitTests.Mocks;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Models;
using AppointmentMaker.Domain.RepositoryContracts;
using Microsoft.Extensions.Options;
using Shouldly;

namespace AppointmentMaker.Application.UnitTests.Features.Schedule.Shift;

public class ScheduleShiftCommandHandlerTests
{
    private readonly IScheduleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleShiftCommandHandlerTests()
    {
        _repository = MockScheduleRepository.GetScheduleRepository().Object;
        _unitOfWork = MockUnitOfWork.GetUnitOfWorkMock().Object;
    }

    [Fact]
    public async Task ShouldReturnProperResult()
    {
        const int firstStartHour = 20;
        const int firstEndHour = 21;

        const int secondStartHour = 22;
        const int secondEndHour = 23;

        const int minutesInHour = 60;

        TimeInterval[] intervals = new TimeInterval[]
        {
            new TimeInterval
            {
                Start = new WeekTime
                {
                    DayOfWeek = DateTime.UtcNow.DayOfWeek,
                    Time = new TimeOnly(firstStartHour, 0)
                },
                End = new WeekTime
                {
                    DayOfWeek = DateTime.UtcNow.DayOfWeek,
                    Time = new TimeOnly(firstEndHour, 0)
                }
            }, 
            new TimeInterval
            {
                Start = new WeekTime
                {
                    DayOfWeek = DateTime.UtcNow.DayOfWeek + 1,
                    Time = new TimeOnly(secondStartHour, 0)
                },
                End = new WeekTime
                {
                    DayOfWeek = DateTime.UtcNow.DayOfWeek + 1,
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

        ScheduleShiftCommandHandler shiftHandler = new ScheduleShiftCommandHandler(_repository, _unitOfWork, options);

        var shiftResut = await shiftHandler.Handle(new ScheduleShiftCommand(), CancellationToken.None);

        shiftResut.IsSuccess.ShouldBe(true);

        ScheduleGetFreeTimeSlotsQueryHandler getHandler
            = new ScheduleGetFreeTimeSlotsQueryHandler
            (_repository, options);

        var getResult = await getHandler.Handle
            (new ScheduleGetFreeTimeSlotsQuery(result.Value, DateOnly.FromDateTime(DateTime.UtcNow)), CancellationToken.None);

        getResult.IsSuccess.ShouldBe(true);

        for (int i = 0; i < (secondEndHour - secondStartHour) * minutesInHour; i += configuration.VisitLengthInMinutes)
        {
            getResult.Value.ShouldContain(new TimeOnly(secondStartHour + i / minutesInHour, i % minutesInHour));
        }

        for (int i = 0; i < (firstEndHour - firstStartHour) * minutesInHour; i += configuration.VisitLengthInMinutes)
        {
            getResult.Value.ShouldNotContain(new TimeOnly(firstStartHour + i / minutesInHour, i % minutesInHour));
        }
    }
}
