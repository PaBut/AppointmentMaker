using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts.Base;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Enums;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.ServiceContracts;

public interface IDoctorService : IUserService
{
    Task<Result<Schedule>> GetDoctorSchedule(string id);
    Task<Result> AddPatient(string doctorId, string patientId);
    Task <Result<CursorResponse<PatientDetails>>> GetPatients(string doctorId, int pageSize, string? cursor);
    Task<Result<DoctorDetails>> GetDetails(string id);
    Task<Result<DoctorFullDetails>> GetFullDetails(string id);
    Task<Result<CursorResponse<DoctorDetails>>> GetDoctors(int pageSize, string? cursor,
        DoctorSortBy? sortBy = null, string? lookBy = null);
    Task<Result> Update(DoctorUpdateRequest request);
    Task<Result> AssignProfilePicture(string doctorId, Guid photoId);
}
