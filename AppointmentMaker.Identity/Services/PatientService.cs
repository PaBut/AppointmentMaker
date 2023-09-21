using AppointmentMaker.Application.Models.Identity;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AppointmentMaker.Identity.Entities.Users;
using AppointmentMaker.Identity.Services.Base;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AppointmentMaker.Identity.Services;

public class PatientService : UserService<Patient>, IPatientService
{
    private readonly IMapper _mapper;
    private readonly IPatientVisitRepository _patientVisitRepository;

    public PatientService(UserManager<Patient> userManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IPatientVisitRepository petVisitRepository)
        : base(userManager, httpContextAccessor)
    {
        _mapper = mapper;
        _patientVisitRepository = petVisitRepository;
    }

    public async Task<Result> AssignToDoctor(string patientId, string doctorId)
    {
        var patient = await _userManager.FindByIdAsync(patientId);

        if(patient == null)
        {
            return Result.Failure(new Error("Patient.AssignToDoctor", "Patient with specified id not found"));
        }

        if (!await UserExists(doctorId))
        {
            return Result.Failure(new Error("Patient.AssignToDoctor", "Doctor with specified id not found"));
        }

        patient.FamilyDoctorId = doctorId;

        await _userManager.UpdateAsync(patient);

        return Result.Success();
    }

    public async Task<Result<PatientDetails>> GetDetails(string id)
    {
        var patient = await _userManager.FindByIdAsync(id);

        if (patient == null)
        {
            return Result.Failure<PatientDetails>(Error.NotFound(nameof(Patient)));
        }

        var patientDetails = _mapper.Map<PatientDetails>(patient);

        return patientDetails;
    }

    public async Task<Result<PatientFullDetails>> GetFullDetails(string id)
    {
        var patient = await _userManager.FindByIdAsync(id);

        if (patient == null)
        {
            return Result.Failure<PatientFullDetails>(Error.NotFound(nameof(Patient)));
        }

        var patientDetails = _mapper.Map<PatientFullDetails>(patient);

        patientDetails.VisitHistory = await _patientVisitRepository.GetPatientVisitHistory(id);

        return patientDetails;
    }

    public async Task<Result> Update(PatientUpdateRequest request)
    {
        string? id = await GetUserId();
        if(id == null)
        {
            return Result.Failure(new Error("Error.UnAuthorized", "UnAuthorized request"));
        }
        
        var patient = await _userManager.FindByIdAsync(id);
        if (patient == null)
        {
            return Result.Failure(new Error("Patient.Update", "Unexpected error"));
        }

        patient.Email = request.Email;
        patient.PhoneNumber = request.PhoneNumber;
        patient.UserName = request.UserName;

        await _userManager.UpdateAsync(patient);

        return Result.Success();
    }
}
