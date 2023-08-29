using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Identity.Services;

public class PatientDoctorConnectionService : IPatientDoctorConnectionService
{
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly IUnitOfWork _unitOfWork;

    public PatientDoctorConnectionService(IDoctorService doctorService,
        IPatientService patientService,
        IUnitOfWork unitOfWork)
    {
        _doctorService = doctorService;
        _patientService = patientService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> CreateConnection(string patientId, string doctorId)
    {
        await _unitOfWork.BeginTransaction();

        var assignToDoctorResult = await _patientService.AssignToDoctor(patientId, doctorId);

        if (assignToDoctorResult.IsFailure)
        {
            await _unitOfWork.RollBackTransaction();
            return Result.Failure(assignToDoctorResult.Error);
        }

        var addPatientResult = await _doctorService.AddPatient(doctorId, patientId);

        if (addPatientResult.IsFailure)
        {
            await _unitOfWork.RollBackTransaction();
            return Result.Failure(assignToDoctorResult.Error);
        }

        await _unitOfWork.CommitTransaction();
        return Result.Success();
    }
}
