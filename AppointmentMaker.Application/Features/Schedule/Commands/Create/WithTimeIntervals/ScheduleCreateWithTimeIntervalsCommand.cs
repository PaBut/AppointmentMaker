using AppointmentMaker.Application.Features.Schedule.Commands.Create.Base;
using AppointmentMaker.Application.Features.Schedule.Commands.Shared;
using AppointmentMaker.Domain.Models;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Create.WithTimeIntervals;

public record ScheduleCreateWithTimeIntervalsCommand(TimeInterval[] TimeIntervals, string DoctorId) 
    : BaseUpsertScheduleCommand(TimeIntervals, DoctorId), IScheduleCreateCommand;
