using AppointmentMaker.Application.Models.Identity.Authentication.Base;
using AppointmentMaker.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace AppointmentMaker.Application.Models.Identity.Authentication;

public class DoctorRegisterWithTimeIntervalsRequest : BaseDoctorRegisterRequest
{
    [Required]
    public TimeInterval[] ScheduleTemplate { get; set; }
}
