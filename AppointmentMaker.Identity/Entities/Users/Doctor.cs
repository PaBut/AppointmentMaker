using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Identity.Entities.Users.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentMaker.Identity.Entities.Users;

public class Doctor : AbstractUser
{
    public Guid? ScheduleId { get; set; }
    [ForeignKey(nameof(ScheduleId))]
    public Schedule? Schedule { get; set; }
    public Guid? PhotoId { get; set; }
    [ForeignKey(nameof(PhotoId))]
    public FileModel? Photo { get; set; }
    public string About { get; set; } = string.Empty;
    public ICollection<Patient> PatientsList { get; set; } = new List<Patient>();
}
