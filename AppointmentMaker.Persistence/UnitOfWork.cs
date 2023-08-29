using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Transactions;

namespace AppointmentMaker.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private TransactionScope? _transaction = null;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public Task BeginTransaction()
    {
        if(_transaction == null)
        {
            _transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            return Task.CompletedTask;
        }
        else
        {
            throw new TransactionException("Unit of Work cant have more than one transactions running at a time");
        }
    }

    public Task CommitTransaction()
    {
        if( _transaction != null)
        {
            _transaction.Complete();
            _transaction.Dispose();
            _transaction = null;
        }
        return Task.CompletedTask;
    }

    public Task RollBackTransaction()
    {
        if(_transaction != null)
        {
            _transaction.Dispose();
            _transaction = null;
        }
        return Task.CompletedTask;
    }
}
