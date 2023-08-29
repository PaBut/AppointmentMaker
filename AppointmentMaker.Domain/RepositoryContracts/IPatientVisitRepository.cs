using AppointmentMaker.Domain.Entities;

namespace AppointmentMaker.Domain.RepositoryContracts;

public interface IPatientVisitRepository : IGenericRepository<PatientVisit>
{
    Task<List<PatientVisit>> GetPatientVisitHistory(string patientId);
}
