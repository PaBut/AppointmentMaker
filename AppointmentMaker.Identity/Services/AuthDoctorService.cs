using AppointmentMaker.Application.Features.FileModel.Commands.RegisterCreate;
using AppointmentMaker.Application.Features.Schedule.Commands.Create.WithBoolArray;
using AppointmentMaker.Application.Features.Schedule.Commands.Create.WithTimeIntervals;
using AppointmentMaker.Application.Models.Identity.Authentication;
using AppointmentMaker.Application.Models.Identity.Authentication.Base;
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

public sealed class AuthDoctorService : AuthService<Doctor, BaseDoctorRegisterRequest>, IAuthDoctorService
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public AuthDoctorService(SignInManager<Doctor> signInManager,
        UserManager<Doctor> userManager,
        IOptions<JwtSettings> jwtSettingsOptions,
        IDoctorService userService,
        IMapper mapper,
        IMediator mediator,
        IUnitOfWork unitOfWork)
        : base(signInManager, userManager,
            jwtSettingsOptions, userService, mapper)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthenticationResponse>> Register<TRegisterRequest>
        (TRegisterRequest request)
        where TRegisterRequest: BaseDoctorRegisterRequest
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

        var createScheduleResult = typeof(TRegisterRequest) == typeof(DoctorRegisterWithBoolArrayRequest) 
            ? await _mediator.Send(new ScheduleCreateWithBoolArrayCommand(
                (request as DoctorRegisterWithBoolArrayRequest)!.ScheduleTemplate, user.Id))
            : await _mediator.Send(new ScheduleCreateWithTimeIntervalsCommand(
                (request as DoctorRegisterWithTimeIntervalsRequest)!.ScheduleTemplate, user.Id));

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
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            Role = RolesEnum.Doctor.ToString(),
            Token = await GenerateJwtToken(user)
        };
    }
}
