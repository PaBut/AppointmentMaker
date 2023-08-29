using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AppointmentMaker.Persistence.Repositories;

public class FileModelRepository : GenericRepository<FileModel>, IFileModelRepository
{
    public FileModelRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<FileModel?> GetByDoctorIdAsync(string doctorId)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.DoctorId == doctorId);
    }
}
