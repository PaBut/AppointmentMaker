using AppointmentMaker.Domain.Enums;

namespace AppointmentMaker.Application.Features.Appointement.Queries.GetDetails;

public class AppointmentDetailsDto
{
    public Guid Id { get; set; }
    public string PatientProblem { get; set; }
    public string ProblemDetails { get; set; }
    public DateTime DateTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public string PatientId { get; set; }
    public string PatientFullName { get; set;}
    public string PatientEmail { get; set;}
}
