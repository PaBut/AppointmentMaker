namespace AppointmentMaker.Application.Features.PatientVisit.Queries.GetVisitDetails;

public class PatientVisitDetailsDto
{
    public Guid Id { get; set; }
    public string PatientProblem { get; set; }
    public string VisitResult { get; set; }
    public DateTime DateTime { get; set; }
    public string DoctorId { get; set; }
    public string DoctorEmail { get; set; }
    public string DoctorFullName { get; set; }
}
