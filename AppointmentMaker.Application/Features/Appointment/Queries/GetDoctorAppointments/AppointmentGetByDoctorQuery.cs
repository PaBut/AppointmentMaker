using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Appointment.Queries.GetDoctorAppointments;

public record AppointmentGetByDoctorQuery(string DoctorId, int PageSize, string? Cursor, DateOnly? Date)
    : IResultRequest<CursorResponse<AppointmentDto>>;
