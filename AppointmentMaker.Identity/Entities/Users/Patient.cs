using AppointmentMaker.Domain.Entities;
using AppointmentMaker.Identity.Entities.Users.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentMaker.Identity.Entities.Users;

public class Patient : AbstractUser
{
    public string? FamilyDoctorId { get; set; }
    [ForeignKey(nameof(FamilyDoctorId))]
    public Doctor? FamilyDoctor { get; set; }
}
