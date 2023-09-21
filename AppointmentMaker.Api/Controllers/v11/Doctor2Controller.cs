using AppointmentMaker.Api.Controllers.Common;
using AppointmentMaker.Api.Extentsions;
using AppointmentMaker.Application.Models.Identity.Authentication;
using AppointmentMaker.Application.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers.v11;

[ApiVersion("1.1")]
public class Doctor2Controller : ApplicationBaseController
{
    private readonly IAuthDoctorService _authService;

    public Doctor2Controller(IAuthDoctorService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [Authorize(Policy = "AllowAnonymousOnly")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterWithTimeIntervals
        (DoctorRegisterWithTimeIntervalsRequest request)
    {
        var result = await _authService.Register(request);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }
        return result.Value;
    }
}
