using System.Text.Json;
using AppointmentMaker.Application.Features.Appointment.Queries.GetDoctorAppointments;
using AppointmentMaker.Domain.RepositoryContracts;
using AutoFixture;
using AutoMapper;
using Moq;
using Shouldly;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppointmentMaker.Application.UnitTests.Features.Appointment.Queries.GetDoctorAppointments;
public class GetDoctorAppointmentsQueryHandlerTests
{

    private List<Domain.Entities.Appointment> _appointments = new();
    private readonly Fixture _fixture;
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly IMapper _mapper;

    public GetDoctorAppointmentsQueryHandlerTests()
    {
        MapperConfiguration mapperConfiguration = new MapperConfiguration
        (config =>
            config.CreateMap<Domain.Entities.Appointment, AppointmentDto>());

        _mapper = mapperConfiguration.CreateMapper();
        _fixture = new Fixture();
        _mockAppointmentRepository = new Mock<IAppointmentRepository>();
    }

    [Fact]
    public async Task ShouldProvideProperOutput()
    {
        const int appointmentCount = 23;
        const int pageSize = 10;

        string doctorId = Guid.NewGuid().ToString();

        for (int i = 0; i < appointmentCount; i++)
        {
            _appointments.Add(
                _fixture.Build<Domain.Entities.Appointment>()
                    .With(a => a.DoctorId, doctorId)
                    .Create()
                );
        }

        List<AppointmentDto>? expectedResult = _mapper.Map<List<AppointmentDto>>(_appointments)
            .OrderBy(e => e.DateTime).ToList();

        _mockAppointmentRepository.Setup(r =>
                r.GetDoctorAppointmentsAsync(It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<string?>()))
            .Returns(Task.FromResult(_appointments.AsEnumerable()));

        var appointmentRepository = _mockAppointmentRepository.Object;

        var resultR = await 
            appointmentRepository.GetDoctorAppointmentsAsync(doctorId, null,
                null);

        var handler = new AppointmentGetByDoctorQueryHandler(appointmentRepository, _mapper);

        var result = await handler.Handle(new AppointmentGetByDoctorQuery
            (doctorId, pageSize, null, null), CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();

        var cursorResponse = result.Value;

        for (int i = 0; i < pageSize; i++)
        {
            cursorResponse.List[i].ShouldBeEquivalentTo(expectedResult[i]);
        }

        var newCursor = cursorResponse.Cursor;
        var newCursorBytes = Convert.FromBase64String(newCursor);

        using StreamReader sr = new StreamReader(new MemoryStream(newCursorBytes));
        JsonSerializer.Deserialize<DateTime>(sr.ReadToEnd())
            .ShouldBe(expectedResult.Skip(pageSize).First().DateTime);

        cursorResponse.HasNextPage.ShouldBeTrue();

        var result2 = await handler.Handle(new AppointmentGetByDoctorQuery
            (doctorId, pageSize, newCursor, null), CancellationToken.None);

        result2.IsSuccess.ShouldBeTrue();

        var cursorResponse2 = result2.Value;

        for (int i = 0; i < pageSize; i++)
        {
            cursorResponse2.List[i].ShouldBeEquivalentTo(expectedResult[i + pageSize]);
        }

        var newCursor2 = cursorResponse2.Cursor;
        var newCursorBytes2 = Convert.FromBase64String(newCursor2);

        using StreamReader sr2 = new StreamReader(new MemoryStream(newCursorBytes2));
        JsonSerializer.Deserialize<DateTime>(sr2.ReadToEnd())
            .ShouldBe(expectedResult.Skip(pageSize * 2).First().DateTime);

        cursorResponse2.HasNextPage.ShouldBeTrue();
    }
}

