using System.ComponentModel.DataAnnotations;

namespace AppointmentMaker.Application.Models.Identity.Base;

public abstract class BaseRegisterRequest
{
    [Required]
    public string FullName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public DateTime Birthday { get; set; }
    [Required]
    public string Password { get; set; } = string.Empty;
}
