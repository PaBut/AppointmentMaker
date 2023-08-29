using AppointmentMaker.Application.Features.Appointement.Commands.Create;
using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Enums;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Appointement.Commands.Cancel;

public class AppointmentCancelCommandHandler : IResultRequestHandler<AppointmentCancelCommand>
{
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IAppointmentRepository _appointementRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public AppointmentCancelCommandHandler(IDoctorService doctorService,
        IOptions<ScheduleConfiguration> scheduleConfigurationOptions,
        IScheduleRepository scheduleRepository,
        IAppointmentRepository appointementRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPatientService patientService)
    {
        _doctorService = doctorService;
        _scheduleConfiguration = scheduleConfigurationOptions.Value;
        _scheduleRepository = scheduleRepository;
        _appointementRepository = appointementRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _patientService = patientService;
    }

    public async Task<Result> Handle(AppointmentCancelCommand command, CancellationToken cancellationToken)
    {
        var appointment = await _appointementRepository.GetByIdAsync(command.Id);

        if (appointment == null)
        {
            return Result.Failure(new Error("Appointment.Cancel", "Appointment with specified id not found"));
        }

        var appointmentCancelResult = appointment.Cancel();
        
        if (appointmentCancelResult.IsFailure)
        {
            return Result.Failure(appointmentCancelResult.Error);
        }

        await _appointementRepository.UpdateAsync(appointment);

        var schedule = await _scheduleRepository.GetByIdAsync(appointment.ScheduleId);

        if (schedule == null)
        {
            return Result.Failure(new Error("Appointment.Cancel", "Unexpected error"));
        }

        var scheduleChangeResult = schedule.UpdateTimeSlot(appointment.DateTime, false, _scheduleConfiguration);
        
        if (scheduleChangeResult.IsFailure)
        {
            return Result.Failure(scheduleChangeResult.Error);
        }

        await _scheduleRepository.UpdateAsync(schedule);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
