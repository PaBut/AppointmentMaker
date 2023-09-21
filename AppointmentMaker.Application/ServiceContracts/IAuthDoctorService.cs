using AppointmentMaker.Application.Models.Identity.Authentication;
using AppointmentMaker.Application.Models.Identity.Authentication.Base;
using AppointmentMaker.Application.ServiceContracts.Base;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Application.ServiceContracts;

public interface IAuthDoctorService : IAuthService
{
    Task<Result<AuthenticationResponse>> Register<TRegisterRequest>
        (TRegisterRequest request)
        where TRegisterRequest : BaseDoctorRegisterRequest;
}
