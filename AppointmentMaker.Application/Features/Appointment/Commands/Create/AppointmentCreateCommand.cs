using AppointmentMaker.Application.Features.Shared;

namespace AppointmentMaker.Application.Features.Appointment.Commands.Create;

public record AppointmentCreateCommand : IResultRequest<Guid>
{
    public string PatientProblem { get; set; }
    public string ProblemDetails { get; set; }
    public DateTime DateTime { get; set; }
    public string DoctorId { get; set; }
}
