using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers;

[ApiVersionNeutral]
public class PatientController : ApplicationBaseController
{
    private readonly IAuthPatientService _authService;

    public PatientController(IAuthPatientService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register(PatientRegisterRequest request)
    {
        var result = await _authService.Register(request);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }
        return result.Value;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
    {
        var result = await _authService.Login(request);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }
        return result.Value;
    }
}
