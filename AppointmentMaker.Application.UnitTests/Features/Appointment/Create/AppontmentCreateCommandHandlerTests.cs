using AppointmentMaker.Application.Features.Appointement.Commands.Create;
using AppointmentMaker.Application.Features.Schedule.Commands.Create.WithTimeIntervals;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Application.UnitTests.Mocks;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Models;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;

namespace AppointmentMaker.Application.UnitTests.Features.Appointment.Create;

public class AppontmentCreateCommandHandlerTests
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IDoctorService> _mockDoctorService;
    private readonly Mock<IPatientService> _mockPatientService;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public AppontmentCreateCommandHandlerTests()
    {
        _scheduleRepository = MockScheduleRepository.GetScheduleRepository().Object;
        _mockAppointmentRepository = new Mock<IAppointmentRepository>();
        _unitOfWork = MockUnitOfWork.GetUnitOfWorkMock().Object;
        _mockDoctorService = new Mock<IDoctorService>();
        _mockPatientService = new Mock<IPatientService>();
        var mapperConfiguration = new MapperConfiguration(cfg =>
        cfg.CreateMap<AppointmentCreateCommand, Domain.Entities.Appointment>());
        _mapper = mapperConfiguration.CreateMapper();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task ShouldBeFailTakingSameDateTime()
    {
        const int startHour = 20;
        const int endHour = 21;

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
            (options, _scheduleRepository, _unitOfWork);

        var result = await handler.Handle
            (new ScheduleCreateWithTimeIntervalsCommand(intervals, Guid.NewGuid().ToString()),
            CancellationToken.None);

        result.IsSuccess.ShouldBe(true);

        _mockDoctorService.Setup(s => s.GetDoctorSchedule(It.IsAny<string>()))
            .Returns((string id) =>
            {
                return Task.FromResult(Result.Success(_scheduleRepository.GetByIdAsync(result.Value).GetAwaiter().GetResult()!));
            });

        _mockDoctorService.Setup(s => s.UserExists(It.IsAny<string>()))
            .Returns((string stringId) => Task.FromResult(true));

        _mockPatientService.Setup(s => s.GetUserId())
            .Returns(() => Task.FromResult(Guid.NewGuid().ToString())!);

        AppointmentCreateCommandHandler appointmentHandler = new AppointmentCreateCommandHandler(_mockDoctorService.Object,
            options,
            _scheduleRepository,
            _mockAppointmentRepository.Object,
            _unitOfWork,
            _mapper,
            _mockPatientService.Object);

        var command1 = _fixture.Build<AppointmentCreateCommand>()
            .With(c => c.DateTime,
            new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 
            DateTime.UtcNow.Day, startHour, 0, 0)).Create();

        var appointment1Result = await appointmentHandler.Handle(command1, CancellationToken.None);

        appointment1Result.IsSuccess.ShouldBe(true);

        var command2 = _fixture.Build<AppointmentCreateCommand>()
            .With(c => c.DateTime,
            new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month,
            DateTime.UtcNow.Day, startHour, 0, 0)).Create();

        var appointment2Result = await appointmentHandler.Handle(command2, CancellationToken.None);

        appointment2Result.IsFailure.ShouldBe(false);
    }
}
