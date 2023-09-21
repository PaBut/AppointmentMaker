using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AppointmentMaker.Application.Models.Identity.Authentication.Base;

public class BaseDoctorRegisterRequest : BaseRegisterRequest
{
    [Required]
    public string About { get; set; } = string.Empty;
    public IFormFile? Photo { get; set; }
}
