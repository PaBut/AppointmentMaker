using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.RepositoryContracts;
using Moq;

namespace AppointmentMaker.Application.UnitTests.Mocks;

public class MockScheduleRepository
{
    public static Mock<IScheduleRepository> GetScheduleRepository()
    {
        List<Schedule> scheduleList = new List<Schedule>();
        var mockRepo = new Mock<IScheduleRepository>();
        mockRepo.Setup(e => e.CreateAsync(It.IsAny<Schedule>()))
            .Returns((Schedule schedule) =>
            {
                scheduleList.Add(schedule);
                return Task.CompletedTask;
            });
        mockRepo.Setup(e => e.UpdateAsync(It.IsAny<Schedule>()))
            .Returns((Schedule schedule) =>
            {
                var scheduleInList = scheduleList.FirstOrDefault(e => e.Id == schedule.Id);
                if (scheduleInList == null) 
                    return Task.CompletedTask;
                scheduleInList.ScheduleSlots = schedule.ScheduleSlots;
                scheduleInList.ScheduleTemplate = schedule.ScheduleTemplate;
                scheduleInList.DoctorId = schedule.DoctorId;
                scheduleInList.ModifiedDate = schedule.ModifiedDate;
                scheduleInList.CreatedDate = schedule.CreatedDate;

                return Task.CompletedTask;
            });

        mockRepo.Setup(e => e.GetAllAsync())
            .Returns(() => Task.FromResult(scheduleList));

        mockRepo.Setup(e => e.GetByIdAsync(It.IsAny<Guid>()))
            .Returns((Guid id) =>
            {
                return Task.FromResult(scheduleList.FirstOrDefault(e => e.Id == id));
            });

        mockRepo.Setup(e => e.DeleteAsync(It.IsAny<Schedule>()))
            .Returns((Schedule schedule) =>
            {
                if (!scheduleList.Contains(schedule))
                {
                    return Task.CompletedTask;
                }
                scheduleList.Remove(schedule);
                return Task.CompletedTask;
            });

        mockRepo.Setup(e => e.DeleteRangeAsync(It.IsAny<IEnumerable<Schedule>>()))
            .Returns((IEnumerable<Schedule> schedules) =>
            {
                if (!schedules.SequenceEqual(scheduleList))
                {
                    return Task.CompletedTask;
                }

                foreach(Schedule schedule in schedules)
                {
                    scheduleList.Remove(schedule);
                }

                return Task.CompletedTask;
            });



        return mockRepo;
    }
}
