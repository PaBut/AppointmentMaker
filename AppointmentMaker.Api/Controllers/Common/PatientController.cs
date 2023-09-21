using AppointmentMaker.Api.Extentsions;
using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.Models.Identity.Authentication;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers.Common;

[ApiVersionNeutral]
public class PatientController : ApplicationBaseController
{
    private readonly IAuthPatientService _authService;
    private readonly IPatientService _patientService;

    public PatientController(IAuthPatientService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [Authorize(Policy = "AllowAnonymousOnly")]
    public async Task<ActionResult<AuthenticationResponse>> Register(PatientRegisterRequest request)
    {
        var result = await _authService.Register(request);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }
        return result.Value;
    }

    [HttpPost("login")]
    [Authorize(Policy = "AllowAnonymousOnly")]
    public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
    {
        var result = await _authService.Login(request);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }
        return result.Value;
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<PatientFullDetails>> GetProfile(string id)
    {
        var result = await _patientService.GetFullDetails(id);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }
        return result.Value;
    }
}
