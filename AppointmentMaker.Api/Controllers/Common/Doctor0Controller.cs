using AppointmentMaker.Api.Extentsions;
using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.Models.Identity.Authentication;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace AppointmentMaker.Api.Controllers.Common;

[ApiVersionNeutral]
public class Doctor0Controller : ApplicationBaseController
{
    private readonly IAuthDoctorService _authService;
    private readonly IDoctorService _doctorService;

    public Doctor0Controller(IAuthDoctorService authService, IDoctorService doctorService)
    {
        _authService = authService;
        _doctorService = doctorService;
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
    public async Task<ActionResult<DoctorFullDetails>> GetProfile(string id)
    {
        var result = await _doctorService.GetFullDetails(id);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }
        return result.Value;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<CursorResponse<DoctorDetails>>> GetDoctors
        (int pageSize, string? cursor = null, DoctorSortBy? sortBy = null, string? lookBy = null)
    {
        var result = await _doctorService.GetDoctors(pageSize, cursor, sortBy, lookBy);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }
        return result.Value;
    }

    [HttpGet("{id:guid}/patients")]
    [Authorize(Roles = "Doctor")]
    public async Task<ActionResult<CursorResponse<PatientDetails>>> GetDoctorPatients(string doctorId, int pageSize, string? cursor)
    {
        var result = await _doctorService.GetPatients(doctorId, pageSize, cursor);
        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message,
                statusCode: result.Error.GetErrorStatusCode());
        }
        return result.Value;
    }
}
