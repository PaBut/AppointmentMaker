using AppointmentMaker.Application.ServiceContracts;
using Moq;

namespace AppointmentMaker.Application.UnitTests.Mocks;

public class MockUnitOfWork
{
    public static Mock<IUnitOfWork> GetUnitOfWorkMock()
    {
        var mock = new Mock<IUnitOfWork>();

        return mock;
    }
}
