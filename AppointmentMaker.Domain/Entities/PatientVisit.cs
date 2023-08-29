using AppointmentMaker.Domain.Entities.Common;

namespace AppointmentMaker.Domain.Entities;

public class PatientVisit : Entity
{
    public string PatientProblem { get; set; }
    public string VisitResult { get; set; }
    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get; set; }
    public string PatientId { get; set; }
    public string DoctorId { get; set;}
    public DateTime DateTime { get; set; }
}
