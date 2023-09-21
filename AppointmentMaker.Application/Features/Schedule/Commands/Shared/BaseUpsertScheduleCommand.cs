using AppointmentMaker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentMaker.Application.Features.Schedule.Commands.Shared;

public abstract record BaseUpsertScheduleCommand(TimeInterval[] TimeIntervals, string DoctorId);