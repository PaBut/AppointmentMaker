using AppointmentMaker.Application.Features.FileModel.Queries.Shared;
using AppointmentMaker.Application.Models.Identity.Base;

namespace AppointmentMaker.Application.Models.Identity;

public class DoctorFullDetails : BaseUserDetails
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public FileModelDto Photo { get; set; }
    public int PatientsCount { get; set; }
    public Guid ScheduleId { get; set; }
}
