namespace AppointmentMaker.Application.Features.Appointement.Queries.GetDoctorAppointments;

public class AppointmentGetByDoctorResponse
{
    public List<AppointmentDto> Appointments { get; set; } = new();
    public DateTime Cursor { get; set; }
}
