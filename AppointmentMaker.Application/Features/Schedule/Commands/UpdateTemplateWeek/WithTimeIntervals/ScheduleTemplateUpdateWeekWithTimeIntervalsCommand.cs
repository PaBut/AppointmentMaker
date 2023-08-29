using AppointmentMaker.Application.Features.Schedule.Commands.Shared;
using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Domain.Models;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateWeek.WithTimeIntervals;

public record ScheduleTemplateUpdateWeekWithTimeIntervalsCommand
    (TimeInterval[] TimeIntervals, string DoctorId) 
    : BaseUpsertScheduleCommand(TimeIntervals, DoctorId), IResultRequest;
