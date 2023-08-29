using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AppointmentMaker.Persistence.Repositories;

public class PatientVisitRepository : GenericRepository<PatientVisit>, 
    IPatientVisitRepository
{
    public PatientVisitRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<PatientVisit>> GetPatientVisitHistory(string patientId)
    {
        return await _dbSet.Where(e => e.PatientId == patientId).ToListAsync();
    }
}
