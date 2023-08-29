using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts.Base;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace AppointmentMaker.Application.ServiceContracts;

public interface IDoctorService : IUserService
{
    Task<Result<Schedule>> GetDoctorSchedule(string id);
    Task<Result> AddPatient(string doctorId, string patientId);
    Task<Result<DoctorDetails>> GetDetails(string id);
    Task<Result<DoctorFullDetails>> GetFullDetails(string id);
    Task<Result> Update(DoctorUpdateRequest request);
    Task<Result> AssignProfilePicture(string doctorId, Guid photoId);
}
