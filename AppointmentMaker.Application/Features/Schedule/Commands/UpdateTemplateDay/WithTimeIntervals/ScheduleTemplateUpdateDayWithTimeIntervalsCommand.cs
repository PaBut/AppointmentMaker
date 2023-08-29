using AppointmentMaker.Application.Features.Schedule.Commands.Shared;
using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Domain.Models;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateDay.WithTimeIntervals;

public record ScheduleTemplateUpdateDayWithTimeIntervalsCommand
    (TimeInterval[] TimeIntervals, DayOfWeek WeekDay, string DoctorId) 
    : BaseUpsertScheduleCommand(TimeIntervals, DoctorId), IResultRequest;
