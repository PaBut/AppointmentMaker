using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.ServiceContracts.Base;

public interface IAuthService
{
    Task<Result<AuthenticationResponse>> Login(LoginRequest request);
}
