using AppointmentMaker.Application.Models.Identity.Authentication;
using AppointmentMaker.Application.Models.Identity.Authentication.Base;
using AppointmentMaker.Application.ServiceContracts.Base;
using AppointmentMaker.Domain.Shared;
using AppointmentMaker.Identity.Entities.Users;
using AppointmentMaker.Identity.Entities.Users.Base;
using AppointmentMaker.Identity.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppointmentMaker.Identity.Services.Base;

public abstract class AuthService<TUser, TRegisterRequest> : IAuthService 
    where TUser : AbstractUser
    where TRegisterRequest : BaseRegisterRequest
{
    protected readonly SignInManager<TUser> _signInManager;
    protected readonly UserManager<TUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AuthService(SignInManager<TUser> signInManager,
        UserManager<TUser> userManager,
        IOptions<JwtSettings> jwtSettingsOptions,
        IUserService userService,
        IMapper mapper)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtSettings = jwtSettingsOptions.Value;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<Result<AuthenticationResponse>> Login(LoginRequest request)
    {
        TUser? user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
        {
            return Result.Failure<AuthenticationResponse>(
                new Error("Error.NotFound", "User with specified email not found"));
        }

        var result = await _signInManager
            .CheckPasswordSignInAsync(user, request.Password, false);

        if(result.Succeeded)
        {
            string role = (await _userManager.GetRolesAsync(user)).Single();
            string token = await GenerateJwtToken(user);

            return new AuthenticationResponse
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Role = role,
                Token = token
            };
        }
        else
        {
            return Result.Failure<AuthenticationResponse>(
                new Error("Error.NotAuthorized", "Email or password is not valid"));
        }
    }

    protected async Task<string> GenerateJwtToken(TUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var role = (await _userManager.GetRolesAsync(user)).Single();

        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("uid", user.Id),
            new Claim(ClaimTypes.Role, role)
        }
        .Union(userClaims);

        SymmetricSecurityKey securityKey = 
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        SigningCredentials credentials = 
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: credentials);

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        return handler.WriteToken(token);
    }

    protected async Task<Result<TUser>> CreateUser(TRegisterRequest request)
    {
        if (!await _userService.IsEmailAvailable(request.Email))
        {
            return Result.Failure<TUser>(new Error("Error.BadRequest", "Email is already taken"));
        }

        TUser user = _mapper.Map<TUser>(request);
        user.CreatedDate = DateTime.UtcNow;

        var createUserResult = await _userManager.CreateAsync(user, request.Password);

        if (!createUserResult.Succeeded)
        {
            string codes = string.Join('|', createUserResult.Errors.Select(e => e.Code));
            string descriptions = string.Join('|', createUserResult.Errors.Select(e => e.Description));

            return Result.Failure<TUser>(new Error(codes, descriptions));
        }

        if (!await _signInManager.CanSignInAsync(user))
        {
            return Result.Failure<TUser>(new Error("Error.Unexpected", "Unexpected error"));
        }

        string role = "";

        if(user is Doctor)
        {
            role = RolesEnum.Doctor.ToString();
        }
        else if(user is Patient)
        {
            role = RolesEnum.Patient.ToString();
        }

        await _userManager.AddToRoleAsync(user, role);

        return user;
    }
}
