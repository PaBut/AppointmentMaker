namespace AppointmentMaker.Application.Models.Identity.Base;

public abstract class BaseUserDetails
{
    public string Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
