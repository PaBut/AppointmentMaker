using AppointmentMaker.Application.Models.Identity.Authentication;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.ServiceContracts.Base;

public interface IAuthService
{
    Task<Result<AuthenticationResponse>> Login(LoginRequest request);
}
