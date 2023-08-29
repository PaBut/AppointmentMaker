using AppointmentMaker.Application.Features.Schedule.Commands.Create.WithTimeIntervals;
using AppointmentMaker.Application.Features.Schedule.Queries.GetFreeTimeSlots;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Application.UnitTests.Mocks;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Models;
using AppointmentMaker.Domain.RepositoryContracts;
using Microsoft.Extensions.Options;
using Shouldly;
using System.CodeDom;

namespace AppointmentMaker.Application.UnitTests.Features.Schedule.Create.WithTimeIntervals;

public class CreateScheduleWithTimeIntervalsHandlerTests
{
    private readonly IScheduleRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateScheduleWithTimeIntervalsHandlerTests()
    {
        _repository = MockScheduleRepository.GetScheduleRepository().Object;
        _unitOfWork = MockUnitOfWork.GetUnitOfWorkMock().Object;
    }

    [Fact]
    public async Task ShouldReturnProperResult()
    {
        const int startHour = 20;
        const int endHour = 21;
        const int minutesInHour = 60;

        TimeInterval[] intervals = new TimeInterval[]
        {
            new TimeInterval
            {
                Start = new WeekTime
                {
                    DayOfWeek = DateTime.UtcNow.DayOfWeek,
                    Time = new TimeOnly(startHour, 0)
                },
                End = new WeekTime
                {
                    DayOfWeek = DateTime.UtcNow.DayOfWeek,
                    Time = new TimeOnly(endHour, 0)
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

        ScheduleGetFreeTimeSlotsQueryHandler getHandler
            = new ScheduleGetFreeTimeSlotsQueryHandler
            (_repository, options);

        var getResult = await getHandler.Handle
            (new ScheduleGetFreeTimeSlotsQuery(result.Value, DateOnly.FromDateTime(DateTime.UtcNow)), CancellationToken.None);

        getResult.IsSuccess.ShouldBe(true);

        for(int i = 0; i < (endHour - startHour) * minutesInHour; i += configuration.VisitLengthInMinutes)
        {
            getResult.Value.ShouldContain(new TimeOnly(startHour + i / minutesInHour, i % minutesInHour));
        }
    }
}
