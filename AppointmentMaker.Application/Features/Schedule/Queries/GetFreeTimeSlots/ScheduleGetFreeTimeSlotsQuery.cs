using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Schedule.Queries.GetFreeTimeSlots;

public record ScheduleGetFreeTimeSlotsQuery(Guid Id, DateOnly Date) : IResultRequest<List<TimeOnly>>;
