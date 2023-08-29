namespace AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisits;

public class PatientVisitDto
{
    public Guid Id { get; set; }
    public string PatientProblem { get; set; }
    public DateTime DateTime { get; set; }
}
