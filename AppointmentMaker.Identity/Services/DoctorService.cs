using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Shared;
using AppointmentMaker.Identity.Entities.Users;
using AppointmentMaker.Identity.Services.Base;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppointmentMaker.Identity.Services;

public class DoctorService : UserService<Doctor>, IDoctorService
{
    private readonly IMapper _mapper;

    public DoctorService(UserManager<Doctor> userManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
        : base(userManager, httpContextAccessor)
    {
        _mapper = mapper;
    }

    public async Task<Result> AddPatient(string doctorId, string patientId)
    {
        var doctor = await _userManager.Users.Include(e => e.PatientsList)
            .FirstOrDefaultAsync(e => e.Id == doctorId);

        if (doctor == null)
        {
            return Result.Failure(new Error("Doctor.AssignToDoctor", "Patient with specified id not found"));
        }

        if (!await UserExists(patientId))
        {
            return Result.Failure(new Error("Doctor.AssignToDoctor", "Doctor with specified id not found"));
        }

        doctor.PatientsList.Add(new Patient { Id = patientId });
        await _userManager.UpdateAsync(doctor);

        return Result.Success();
    }

    public async Task<Result> AssignProfilePicture(string doctorId, Guid photoId)
    {
        var doctor = await _userManager.FindByIdAsync(doctorId);

        if (doctor == null)
        {
            return Result.Failure(new Error("Doctor.AssignProfilePicture", "Doctor wth specified id not found"));
        }

        doctor.PhotoId = photoId;
        await _userManager.UpdateAsync(doctor);

        return Result.Success();
    }

    public async Task<Result<DoctorDetails>> GetDetails(string id)
    {
        var doctor = await _userManager.FindByIdAsync(id);

        if (doctor == null)
        {
            return Result.Failure<DoctorDetails>(new Error("Doctor.GetDetails", "Doctor with specified id not found"));
        }

        var doctorDetails = _mapper.Map<DoctorDetails>(doctor);

        return doctorDetails;
    }

    public async Task<Result<Schedule>> GetDoctorSchedule(string id)
    {
        var user = await _userManager.Users
            .Include(u => u.Schedule)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return Result.Failure<Schedule>(new Error("Doctor.GetSchedule",
                "Doctor with specified id not found"));
        }

        var schedule = user.Schedule;

        if (schedule == null)
        {
            return Result.Failure<Schedule>(new Error("Doctor.GetSchedule",
                "Doctor doesnt have own schedule"));
        }

        return schedule!;
    }

    public async Task<Result<DoctorFullDetails>> GetFullDetails(string id)
    {
        var doctor = await _userManager.Users
            .Include(e => e.PatientsList)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (doctor == null)
        {
            return Result.Failure<DoctorFullDetails>(new Error("Doctor.GetFullDetails", "Doctor with specified id not found"));
        }

        var doctorDetails = _mapper.Map<DoctorFullDetails>(doctor);

        //var photoResult = await _mediator.Send(new GetDoctorProfilePictureQuery(id));

        //if (photoResult.IsFailure)
        //{
        //    return Result.Failure<DoctorFullDetails>(photoResult.Error);
        //}

        //doctorDetails.Photo = photoResult.Value;
        doctorDetails.PatientsCount = doctor.PatientsList.Count();

        return doctorDetails;
    }

    public async Task<Result> Update(DoctorUpdateRequest request)
    {
        string? id = await GetUserId();
        if (id == null)
        {
            return Result.Failure(new Error("Doctor.Update", "Unauthoried request"));
        }

        var doctor = await _userManager.FindByIdAsync(id);
        if (doctor == null)
        {
            return Result.Failure(new Error("Doctor.Update", "Unexpected error"));
        }

        doctor.Email = request.Email;
        doctor.PhoneNumber = request.PhoneNumber;
        doctor.UserName = request.UserName;

        await _userManager.UpdateAsync(doctor);
        return Result.Success();
    }
}
