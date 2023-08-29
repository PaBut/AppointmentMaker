using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentMaker.Identity.Entities.Users.Base;

public abstract class AbstractUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
}
