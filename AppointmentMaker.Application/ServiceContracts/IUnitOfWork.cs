namespace AppointmentMaker.Application.ServiceContracts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    Task BeginTransaction();
    Task CommitTransaction();
    Task RollBackTransaction();
}
