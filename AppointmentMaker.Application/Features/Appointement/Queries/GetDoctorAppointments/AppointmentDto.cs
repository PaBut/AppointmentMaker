using AppointmentMaker.Domain.Enums;

namespace AppointmentMaker.Application.Features.Appointement.Queries.GetDoctorAppointments;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public string PatientProblem { get; set; }
    public DateTime DateTime { get; set; }
    public AppointmentStatus Status { get; set; }
}
