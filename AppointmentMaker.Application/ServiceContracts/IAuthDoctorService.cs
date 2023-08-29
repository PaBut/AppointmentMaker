using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts.Base;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.ServiceContracts;

public interface IAuthDoctorService : IAuthService
{
    Task<Result<AuthenticationResponse>> Register(DoctorRegisterRequest request);
}
