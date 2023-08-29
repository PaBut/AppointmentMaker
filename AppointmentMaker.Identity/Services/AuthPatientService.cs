using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Application.ServiceContracts.Base;
using AppointmentMaker.Domain.Shared;
using AppointmentMaker.Identity.Entities.Users;
using AppointmentMaker.Identity.Enums;
using AppointmentMaker.Identity.Services.Base;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Identity.Services;

public class AuthPatientService : AuthService<Patient, PatientRegisterRequest>, IAuthPatientService
{
    public AuthPatientService(SignInManager<Patient> signInManager,
        IHttpContextAccessor httpContextAccessor, 
        UserManager<Patient> userManager,
        IOptions<JwtSettings> jwtSettingsOptions,
        IPatientService userService, IMapper mapper) 
        : base(signInManager, httpContextAccessor, userManager, 
            jwtSettingsOptions, userService, mapper)
    {
    }

    public async Task<Result<AuthenticationResponse>> Register(PatientRegisterRequest request)
    {
        Result<Patient> userResult = await CreateUser(request);

        if (userResult.IsFailure)
        {
            return Result.Failure<AuthenticationResponse>(userResult.Error);
        }

        Patient user = userResult.Value;

        return new AuthenticationResponse
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            Role = RolesEnum.Patient.ToString(),
            Token = await GenerateJwtToken(user)
        };
    }
}
