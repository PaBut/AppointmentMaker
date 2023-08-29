using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Appointement.Queries.GetDoctorAppointments;

public record AppointmentGetAllByDoctorQuery(string DoctorId, int PageSize, DateTime? Cursor, DateOnly? Date)
    : IResultRequest<AppointmentGetByDoctorResponse>;
