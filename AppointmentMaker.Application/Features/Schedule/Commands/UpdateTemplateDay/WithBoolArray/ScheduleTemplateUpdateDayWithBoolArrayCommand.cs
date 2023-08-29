using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateDay.WithBoolArray;

public record ScheduleTemplateUpdateDayWithBoolArrayCommand
    (bool[] ScheduleTemplate, DayOfWeek WeekDay, string DoctorId) 
    : IResultRequest;
