using AppointmentMaker.Domain.Entities;

namespace AppointmentMaker.Domain.RepositoryContracts;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetScheduleAppointmentsAsync(Guid scheduleId);
    Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(string doctorId, DateOnly? date = null, string? orderBy = null);

}
