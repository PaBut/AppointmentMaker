using System.Net;
using AppointmentMaker.Domain.Entities.Common;
using AppointmentMaker.Domain.Enums;
using AppointmentMaker.Domain.Shared;

namespace AppointmentMaker.Domain.Entities;

public class Appointment : Entity
{
    public string PatientProblem { get; set; }
    public string ProblemDetails { get; set; }
    public DateTime DateTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public string DoctorId { get; set; }
    public string PatientId { get; set; }
    public Guid ScheduleId { get; set; }
    public Schedule Schedule { get; set; }

    public Result Cancel()
    {
        if (DateTime < DateTime.UtcNow || Status == AppointmentStatus.Completed)
        {
            return Result.Failure(new Error("Error.BadRequest", "Cannot cancel completed appointment"));
        }

        if (Status == AppointmentStatus.Cancelled)
        {
            return Result.Failure(new Error("Error.BadRequest", "Cannot cancel cancelled appointment"));
        }

        Status = AppointmentStatus.Cancelled;

        return Result.Success();
    }
}
