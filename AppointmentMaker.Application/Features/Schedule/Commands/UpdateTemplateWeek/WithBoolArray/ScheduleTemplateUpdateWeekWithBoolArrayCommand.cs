using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Schedule.Commands.UpdateTemplateWeek.WithBoolArray;

public record ScheduleTemplateUpdateWeekWithBoolArrayCommand(bool[] ScheduleTemplate, string DoctorId) : IResultRequest;
