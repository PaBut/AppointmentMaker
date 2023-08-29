using AppointmentMaker.Application.Features.Shared;
using AppointmentMaker.Domain.Enums;

namespace AppointmentMaker.Application.Features.Schedule.Queries.GetFreeDays;

public record ScheduleGetFreeDaysQuery(Guid Id, Months Month, int Year) : IResultRequest<List<int>>;
