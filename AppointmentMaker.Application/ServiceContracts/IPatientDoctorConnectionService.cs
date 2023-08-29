using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.ServiceContracts;

public interface IPatientDoctorConnectionService
{
    Task<Result> CreateConnection(string patientId, string doctorId);
}
