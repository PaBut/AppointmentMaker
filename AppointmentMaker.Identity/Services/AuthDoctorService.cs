using AppointmentMaker.Application.Features.FileModel.Commands.RegisterCreate;
using AppointmentMaker.Application.Features.Schedule.Commands.Create.WithBoolArray;
using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Shared;
using AppointmentMaker.Identity.Entities.Users;
using AppointmentMaker.Identity.Enums;
using AppointmentMaker.Identity.Services.Base;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Identity.Services;

public class AuthDoctorService : AuthService<Doctor, DoctorRegisterRequest>, IAuthDoctorService
{
    private readonly ScheduleConfiguration _scheduleConfiguration;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public AuthDoctorService(SignInManager<Doctor> signInManager,
        IHttpContextAccessor httpContextAccessor,
        UserManager<Doctor> userManager,
        IOptions<JwtSettings> jwtSettingsOptions,
        IOptions<ScheduleConfiguration> scheduleConfigurationOptions,
        IDoctorService userService,
        IMapper mapper,
        IMediator mediator,
        IUnitOfWork unitOfWork)
        : base(signInManager, httpContextAccessor, userManager,
            jwtSettingsOptions, userService, mapper)
    {
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthenticationResponse>> Register(DoctorRegisterRequest request)
    {
        //if (!await _userService.IsEmailAvailable(request.Email))
        //{
        //    return Result.Failure<AuthenticationResponse>(new Error("User.Register", "Email is already taken"));
        //}

        //Doctor user = _mapper.Map<Doctor>(request);

        //var createUserResult = await _userManager.CreateAsync(user);

        //if (!createUserResult.Succeeded)
        //{
        //    string codes = string.Join('|', createUserResult.Errors.Select(e => e.Code));
        //    string descriptions = string.Join('|', createUserResult.Errors.Select(e => e.Description));

        //    return Result.Failure<AuthenticationResponse>(new Error(codes, descriptions));
        //}

        //if (!await _signInManager.CanSignInAsync(user))
        //{
        //    return Result.Failure<AuthenticationResponse>(new Error("User.Register", "Unexpected error"));
        //}

        //await _userManager.AddToRoleAsync(user, RolesEnum.Doctor.ToString());

        await _unitOfWork.BeginTransaction();

        Result<Doctor> userResult = await CreateUser(request);

        if (userResult.IsFailure)
        {
            await _unitOfWork.RollBackTransaction();
            return Result.Failure<AuthenticationResponse>(userResult.Error);
        }

        Doctor user = userResult.Value;

        var createScheduleResult = await _mediator.Send(new ScheduleCreateWithBoolArrayCommand(request.ScheduleTemplate, user.Id));

        if (createScheduleResult.IsFailure)
        {
            await _unitOfWork.RollBackTransaction();
            return Result.Failure<AuthenticationResponse>(createScheduleResult.Error);
        }

        Guid scheduleId = createScheduleResult.Value;

        user.ScheduleId = scheduleId;

        if(request.Photo != null)
        {
            var uploadPhotoResult = await _mediator.Send(new FileModelRegisterCreateCommand(request.Photo));
            if (uploadPhotoResult.IsFailure)
            {
                await _unitOfWork.RollBackTransaction();
                return Result.Failure<AuthenticationResponse>(uploadPhotoResult.Error);
            }
            user.PhotoId = uploadPhotoResult.Value;
        }

        await _userManager.UpdateAsync(user);

        await _unitOfWork.CommitTransaction();

        return new AuthenticationResponse
        {
            Email = user.Email!,
            UserName = user.UserName!,
            Role = RolesEnum.Doctor.ToString(),
            Token = await GenerateJwtToken(user)
        };
    }
}
