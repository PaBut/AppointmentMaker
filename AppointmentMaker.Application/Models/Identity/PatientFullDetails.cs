using AppointmentMaker.Application.Models.Identity.Base;
using AppointmentMaker.Domain.Entities;

namespace AppointmentMaker.Application.Models.Identity;

public class PatientFullDetails : BaseUserDetails
{
    public string PhoneNumber { get; set; }
    public DateOnly Birthday { get; set; }
    public List<PatientVisit> VisitHistory { get; set; } = new();
}
