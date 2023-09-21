using AppointmentMaker.Application.Features.Appointment.Queries.GetDoctorAppointments;
using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Enums;
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
            return Result.Failure(Error.NotFound(nameof(Patient)));
        }

        if (!await UserExists(patientId))
        {
            return Result.Failure(Error.NotFound(nameof(Doctor)));
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
            return Result.Failure(Error.NotFound(nameof(Doctor)));
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
            return Result.Failure<DoctorDetails>(Error.NotFound(nameof(Doctor)));
        }

        var doctorDetails = _mapper.Map<DoctorDetails>(doctor);

        return doctorDetails;
    }

    public async Task<Result<CursorResponse<DoctorDetails>>> GetDoctors
        (int pageSize, string? cursor, DoctorSortBy? sortBy = null, string? lookBy = null) 
    {
        var doctorQuery = _userManager.Users;

        if(lookBy != null)
        {
            doctorQuery = doctorQuery.Where(e => e.FullName.Contains(lookBy) 
            || e.Email!.Contains(lookBy));
        }

        var doctors = await doctorQuery.AsNoTracking().ToListAsync();

        (bool isAscending, string sortName) = sortBy switch
        {
            DoctorSortBy.NameAsc => (true, nameof(Doctor.FullName)),
            DoctorSortBy.NameDesc => (false, nameof(Doctor.FullName)),
            DoctorSortBy.RecentlyAdded => (true, nameof(Doctor.CreatedDate)),
            DoctorSortBy.LatelyAdded => (false, nameof(Doctor.CreatedDate)),
            _ => (true, nameof(Doctor.Id))
        };

        var result = CursorResponse<DoctorDetails>.GetCursorResponse(doctors,
            pageSize,
            cursor,
            nameof(Doctor.FullName),
            true,
            _mapper);

        if (result.IsFailure)
        {
            return Result.Failure<CursorResponse<DoctorDetails>>(result.Error);
        }

        return result.Value;
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

    public async Task<Result<CursorResponse<PatientDetails>>> GetPatients(string doctorId, int pageSize, string? cursor)
    {
        var doctor = await _userManager.FindByIdAsync(doctorId);

        if(doctor == null)
        {
            return Result.Failure<CursorResponse<PatientDetails>>(new Error("", "Doctor with specified id not found"));
        }

        var result = CursorResponse<PatientDetails>.GetCursorResponse(doctor.PatientsList,
            pageSize,
            cursor,
            nameof(Patient.Id),
            true, 
            _mapper);

        if (result.IsFailure)
        {
            return Result.Failure<CursorResponse<PatientDetails>>(result.Error);
        }

        return result.Value;
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
