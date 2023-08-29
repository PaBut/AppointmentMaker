using System.ComponentModel.DataAnnotations;

namespace AppointmentMaker.Application.Models.Identity.Base;

public abstract class BaseUpdateRequest
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
    [Required]
    public string UserName { get; set; } = string.Empty;
}
