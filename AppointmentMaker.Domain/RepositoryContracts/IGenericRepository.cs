using AppointmentMaker.Domain.Entities.Common;

namespace AppointmentMaker.Domain.RepositoryContracts;

public interface IGenericRepository<T> where T : Entity
{
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
}
