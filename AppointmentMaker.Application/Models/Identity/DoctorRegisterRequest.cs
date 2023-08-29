using AppointmentMaker.Application.Models.Identity.Base;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppointmentMaker.Application.Models.Identity;

public class DoctorRegisterRequest : BaseRegisterRequest
{
    [Required]
    public bool[] ScheduleTemplate { get; set; }
    [Required]
    public string About { get; set; } = string.Empty;
    public IFormFile? Photo { get; set; } 
}
