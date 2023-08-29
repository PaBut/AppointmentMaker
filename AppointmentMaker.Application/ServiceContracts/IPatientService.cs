using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts.Base;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.ServiceContracts;

public interface IPatientService : IUserService
{
    Task<Result> AssignToDoctor(string patientId, string doctorId);
    Task<Result<PatientDetails>> GetDetails(string id);
    Task<Result<PatientFullDetails>> GetFullDetails(string id);
    Task<Result> Update(PatientUpdateRequest request);
}
