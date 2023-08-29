using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentMaker.Api.Controllers;

[ApiVersionNeutral]
public class DoctorController : ApplicationBaseController
{
    private readonly IAuthDoctorService _authService;

    public DoctorController(IAuthDoctorService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [Authorize(Policy = "AllowAnonymousOnly")]
    public async Task<ActionResult<AuthenticationResponse>> Register(DoctorRegisterRequest request)
    {
        var result = await _authService.Register(request);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message);
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
            return Problem(title: result.Error.Code, detail: result.Error.Message);
        }
        return result.Value;
    }
}
