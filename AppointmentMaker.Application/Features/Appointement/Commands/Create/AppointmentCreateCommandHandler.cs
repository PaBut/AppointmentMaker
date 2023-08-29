using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Application.ServiceContracts;
using AppointmentMaker.Domain.Configuration;
using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Domain.Enums;
using AppointmentMaker.Domain.RepositoryContracts;
using AppointmentMaker.Domain.Shared;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace AppointmentMaker.Application.Features.Appointement.Commands.Create;

public class AppointmentCreateCommandHandler : IResultRequestHandler<AppointmentCreateCommand, Guid>
{
    private readonly IDoctorService _doctorService;
    private readonly IPatientService _patientService; 
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IAppointmentRepository _appointementRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ScheduleConfiguration _scheduleConfiguration;

    public AppointmentCreateCommandHandler(IDoctorService doctorService,
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

    public async Task<Result<Guid>> Handle(AppointmentCreateCommand command, CancellationToken cancellationToken)
    {
        var validator = new AppointmentCreateCommandValidator(_doctorService, _scheduleConfiguration);
        var validationResult = await validator.ValidateAsync(command);

        if(!validationResult.IsValid)
        {
            return Result.Failure<Guid>(Error.FromValidationResult(validationResult));
        }

        var schedule = (await _doctorService.GetDoctorSchedule(command.DoctorId)).Value;
        //int dayOffset = (command.DateTime - DateTime.UtcNow).Days;
        //int index = dayOffset * _scheduleConfiguration.SlotsInDay + _scheduleConfiguration.GetDayIndex(command.DateTime);
        //int offset = (command.DateTime.TimeOfDay - _scheduleConfiguration.GetTimeByIndex(index)).Minutes / _scheduleConfiguration.VisitLengthInMinutes;

        //if (((schedule.ScheduleSlots[index] >> (byte)offset) & 1) == 1)
        //{
        //    return Result.Failure(new Error("Appointment.Create", "Time slot is already taken"));
        //}

        //schedule.ScheduleSlots[index] |= (byte)(1 << offset);
        var scheduleChangeResult = schedule.UpdateTimeSlot(command.DateTime, true, _scheduleConfiguration);
        if (scheduleChangeResult.IsFailure)
        {
            return Result.Failure<Guid>(scheduleChangeResult.Error);
        }

        await _scheduleRepository.UpdateAsync(schedule);

        Appointment appointment = _mapper.Map<Appointment>(command);

        appointment.PatientId = (await _patientService.GetUserId())!;
        appointment.Status = AppointmentStatus.Pending;
        appointment.ScheduleId = schedule.Id;

        await _appointementRepository.CreateAsync(appointment);

        await _unitOfWork.SaveChangesAsync();

        return appointment.Id;
    }
}
