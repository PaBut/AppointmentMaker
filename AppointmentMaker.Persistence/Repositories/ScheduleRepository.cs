using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Persistence.DatabaseContext;

namespace AppointmentMaker.Persistence.Repositories;

public class ScheduleRepository : GenericRepository<Schedule>,
    IScheduleRepository
{
    public ScheduleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
