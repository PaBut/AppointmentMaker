using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Extentions;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AppointmentMaker.Persistence.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, 
    IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync
        (string doctorId, DateOnly? date = null, string? orderBy = null)
    {
        var query = _dbSet.AsQueryable();
        if(date != null)
        {
            query = query.Where(e => e.DateTime.IsDate(date.Value));
        }
        if(orderBy != null)
        {
            query = OrderByProperty(query, orderBy);
        }

        return Task.FromResult(query.Where(e => e.DoctorId == doctorId).AsEnumerable());
    }

    public async Task<IEnumerable<Appointment>> GetScheduleAppointmentsAsync(Guid scheduleId) =>
        await _dbSet.Where(e => e.ScheduleId == scheduleId)
            .AsNoTracking()
            .ToListAsync();
}
