using AppointmentMaker.Domain.Entities.Common;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentMaker.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : Entity
    {
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync() =>
            await _dbSet.AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id) =>
            await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        protected IQueryable<T> OrderByProperty(IQueryable<T> query, string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);

            if (propertyInfo != null)
            {
                return query.OrderBy(e => propertyInfo.GetValue(e, null));
            }

            return query;
        }
    }
}
