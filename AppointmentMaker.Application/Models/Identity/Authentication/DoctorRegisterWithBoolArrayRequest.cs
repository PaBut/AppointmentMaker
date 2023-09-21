using AppointmentMaker.Application.Models.Identity.Authentication.Base;
using System.ComponentModel.DataAnnotations;

namespace AppointmentMaker.Application.Models.Identity.Authentication;

public class DoctorRegisterWithBoolArrayRequest : BaseDoctorRegisterRequest
{
    [Required]
    public bool[] ScheduleTemplate { get; set; }
}